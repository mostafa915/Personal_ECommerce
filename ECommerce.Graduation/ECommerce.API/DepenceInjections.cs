using ECommerce.Application.Authentication;
using ECommerce.Application.IRepos;
using ECommerce.Application.Services;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Configuration;
using ECommerce.Infrastructure.Repos;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ECommerce.API
{
    public static class DepenceInjections
    {
        public static IServiceCollection AddDepnces(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenApi();
            services.AddDatabase(configuration);
            services.AddGlobalExceptionHandler();
            services.AddCorsBroswer();
            services.AddFluentValidation();
            services.AddServices();
            services.AddAuthConfig(configuration);
            services.AddMapster();
            return services;
        }


        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConncetion") ??
                throw new InvalidOperationException("Error In DefaultConnection!!");

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)
            );

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            return services;

        }
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepo, AuthRepo>();
            services.AddScoped<IJwt, Jwt>();
            services.AddScoped<IAuthService, AuthService>();
            
            return services;
        }

        private static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }

        private static IServiceCollection AddCorsBroswer(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });
            return services;
        }
        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(typeof(ECommerce.Application.IApplicationMarker).Assembly);
            return services;
        }

        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //Ioptions
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                        ValidIssuer = jwtSettings?.Issuer,
                        ValidAudience = jwtSettings?.Audience

                    };
                });


            // End Jwt Configuration

            // Strat Confinguration to manage register and login in

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            }
            );

            return services;
        }
       
        private static IServiceCollection AddMapster(this IServiceCollection services)
        {

            var mappingConfing = TypeAdapterConfig.GlobalSettings;
            mappingConfing.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfing));
            return services;
        }

    }
}
