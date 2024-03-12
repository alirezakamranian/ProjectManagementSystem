
using Application.Services;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Domain.Services;
using Domain.Entities.HumanResource;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        //AuthAndIdentity
        public static void ConfigureAuth(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();


            // Authentication &  Jwt Bearer
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {

                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["AuthOptions:IssuerAudience"],
                    ValidIssuer = builder.Configuration["AuthOptions:IssuerAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthOptions:Key"]))
                };
            });
        }
        //AuthService
        public static void ConfigureAuthenticationservice(this IServiceCollection services)
        {
             services.AddTransient<IAuthenticationService, AuthenticationService>();
        }
    }
}
