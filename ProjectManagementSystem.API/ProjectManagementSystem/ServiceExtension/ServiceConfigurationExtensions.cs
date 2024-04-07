using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Services.ApiServices;
using Application.Services.ApiServices;
using Application.Services.InternalServices;
using Domain.Services.InternalServices;
using Serilog;
using Domain.Entities.HumanResource;
namespace ProjectManagementSystem.ServiceExtension
{
    public static class ServiceConfigurationExtensions
    {
        //Mvc
        public static void ConfigureMvc(this IServiceCollection services) =>
            services.AddControllers();

        //Logger
        public static void ConfigureLogging(this IHostBuilder hostBuilder) 
        {
            hostBuilder.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));
        }

        //DbContext
        public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder) =>
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
                    b => b.MigrationsAssembly("ProjectManagementSystem")));

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
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                       .GetBytes(builder.Configuration["AuthOptions:Key"]))
                };
            });
        }

        //ApiServices
        public static void ConfigureAppServices(this IServiceCollection services)
        {
            /*ApiServices*/

            //AuthService
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            //OrgService
            services.AddTransient<IOrganizationService, OrganizationService>();
            //OrgInvitation
            services.AddTransient<IOrganizationInvitationService, OrganizationInvitationService>();
            //UserService
            services.AddTransient<IUserService, UserService>();
            //ProjectService
            services.AddTransient<IProjectService, ProjectService>();
            //OrgEmployeeService
            services.AddTransient<IOrganizationEmployeeService, OrganizationEmployeeService>();
            //NotificationService
            services.AddTransient<IUserNotificationService, UserNotificationService>();
            //ProjectMemberService
            services.AddTransient<IProjectMemberService, ProjectMemberService>();

            /*InternalServices*/

            //TokenGeneratorw
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            //InvitationService
            services.AddTransient<IInvitationPendingManager, InvitationPendingManager>();
        }
    }
}
