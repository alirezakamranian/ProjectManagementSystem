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
using Npgsql;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
namespace ProjectManagementSystem.ServiceExtension
{
    public static class ServiceConfigurationExtensions
    {
        //Mvc
        public static void ConfigureMvc(this IServiceCollection services) =>
            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters
                    .Add(new JsonStringEnumConverter()));

        //Logger
        public static void ConfigureLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));
        }

        //DbContext
        public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder) =>
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PGSQL"),
                    o => o.MigrationsAssembly("ProjectManagementSystem")));

        //Swagger
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "PMS API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                     {
                        new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type=ReferenceType.SecurityScheme,
                                 Id="Bearer"
                             }
                        },
                        new string[]{}
                     }
                });
            });
        }

        //Cors
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("reactApp", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
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
            //ProjectTaskListService
            services.AddTransient<IProjectTaskListService, ProjectTaskListService>();

            /*InternalServices*/

            //TokenGeneratorw
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            //InvitationService
            services.AddTransient<IInvitationPendingManager, InvitationPendingManager>();
        }
    }
}
