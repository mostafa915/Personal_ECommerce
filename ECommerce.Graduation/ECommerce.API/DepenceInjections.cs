using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Configuration;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
