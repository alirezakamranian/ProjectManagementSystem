
using Application.Services;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Domain.Services;
namespace ProjectManagementSystem.ServiceExtension
{
    public static class ServiceConfigurationExtensions
    {
        //Mvc
        public static void ConfigureMvc(this IServiceCollection services) =>
            services.AddControllers();
        //DbContext
        public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder) =>
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
        //Swagger
        public static void ConfigureSwagger(this IServiceCollection services) 
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        //AuthService
        public static void ConfigureAuthenticationservice(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
        }
    }
}
