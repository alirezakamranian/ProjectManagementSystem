using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.HumanResource;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Services.ApiServices;
using Application.Services.ApiServices;
using Application.Services.InternalServices;
using Domain.Services.InternalServices;
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
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), b => b.MigrationsAssembly("ProjectManagementSystem")));

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

        //ApiServices
        public static void ConfigureApiServices(this IServiceCollection services)
        {
            //AuthService
             services.AddTransient<IAuthenticationService, AuthenticationService>();
            //OrgService
             services.AddTransient<IOrganizationService, OrganizationService>();
            //OrgInvitation
             services.AddTransient<IOrganizationInvitationService, OrganizationInvitationService>();

            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IInvitationPendingManager, InvitationPendingManager>();
        }

        //InternalServices
        public static void ConfigureInternalServices(this IServiceCollection services)
        {
            //TokenGenerator
             //services.AddTransient<ITokenGenerator, TokenGenerator>();
            //InvitationManager
             //services.AddTransient<IInvitationPendingManager, InvitationPendingManager>();
        }
    }
}
