using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

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
    }
}
