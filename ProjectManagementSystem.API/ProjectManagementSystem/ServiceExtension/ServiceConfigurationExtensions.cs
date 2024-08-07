﻿using Infrastructure.DataAccess;
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
        /// <summary>
        /// Configures MVC pattern for presetation layer
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMvc(this IServiceCollection services) =>
            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters
                    .Add(new JsonStringEnumConverter()));

        //Logger
        /// <summary>
        /// Configures Serilog library for logging
        /// </summary>
        /// <param name="hostBuilder"></param>
        public static void ConfigureLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));
        }

        //DbContext
        /// <summary>
        /// Configures DbContext (base of communication by EF ORM to DB)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
        public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder)
        {
            if (builder.Configuration["DataBase"] == "CloudDb")
            {
                services.AddDbContext<DataContext>(options =>
                     options.UseNpgsql(builder.Configuration.GetConnectionString("PGSQL"),
                         o => o.MigrationsAssembly("ProjectManagementSystem")));
            }

            if (builder.Configuration["DataBase"] == "LocalDb")
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(builder.Configuration
                        .GetConnectionString("SqlServer"), o => o
                            .MigrationsAssembly("ProjectManagementSystem")));
            }
        }

        //Swagger
        /// <summary>
        /// Configures Swagger fo openAPI documentation
        /// </summary>
        /// <param name="services"></param>
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
                        Array.Empty<string>()
                     }
                });
            });
        }

        //Cors
        /// <summary>
        /// Configures Cors that allows another apps send request to this app 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("Allow", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

        //AuthAndIdentity
        /// <summary>
        /// Configures Authentication services and security basics
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
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

                options.Authority = "Authority URL";

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/notifhub")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        //ApiServices
        /// <summary>
        /// Configures all services written in Application layer
        /// </summary>
        /// <param name="services"></param>
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
            //ProjectTaskService
            services.AddTransient<IProjectTaskService, ProjectTaskService>();
            //TaskAssignmentService
            services.AddTransient<ITaskAssignmentService, TaskAssignmentService>();
            //StorageService
            services.AddTransient<IStorageService, StorageService>();
            //TaskCommentService
            services.AddTransient<ITaskCommentService, TaskCommentService>();
            //TaskLabelManagementService
            services.AddTransient<ITaskLableManagementService, TaskLableManagementService>();
            //TaskLabelAttachment
            services.AddTransient<ITaskLabelAttachmentService, TaskLabelAttachmentService>();

            /*InternalServices*/
            
            //TokenGeneratorw
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            //InvitationPendingService
            services.AddTransient<IInvitationPendingManager, InvitationPendingManager>();
            //AuthorizationService
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            //RealTimeNotificationService
            services.AddTransient<IRealTimeNotificationService, RealTimeNotificationService>();
            //RealTimeTaskService
            services.AddTransient<IRealTimeTaskService, RealTimeTaskService>();
        }
    }
}
