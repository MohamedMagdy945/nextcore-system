using Auth.Application.Interfaces;
using Auth.Infrastructure.Interfaces;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Persistence.DatabaseSeeder;
using Auth.Infrastructure.Persistence.Seeder;
using Auth.Infrastructure.Services;
using Auth.Infrastructure.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Text;
namespace Auth.Infrastructure
{
    public static class InfrastructureRegistrationService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<IAuthDbContext, AuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.LogTo(Console.WriteLine, LogLevel.Information);
            });

            services.Configure<JwtSettings>(
                 configuration.GetSection("JwtSettings"));

            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();

            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

            services.AddScoped<IClientInfoProvider, ClientInfoProvider>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<RoleSeeder>();
            services.AddScoped<PermissionSeeder>();
            services.AddScoped<UserSeeder>();
            services.AddScoped<DatabaseSeeder>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
           .AddJwtBearer("Bearer", options =>
           {

               var jwtSettings = configuration
               .GetSection("JwtSettings")
               .Get<JwtSettings>();

               if (jwtSettings == null)
                   throw new InvalidOperationException("JWT settings are not configured properly.");

               options.RequireHttpsMetadata = false;
               options.SaveToken = true;

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidIssuer = jwtSettings.Issuer,

                   ValidateAudience = true,
                   ValidAudience = jwtSettings.Audience,

                   ValidateLifetime = true,

                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret)),

                   ClockSkew = TimeSpan.Zero
               };

               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       Console.WriteLine(context.Exception);

                       var logger = context.HttpContext
                           .RequestServices
                           .GetRequiredService<ILoggerFactory>()
                           .CreateLogger("JWT");

                       logger.LogWarning(
                           "JWT Authentication failed: {Message} | Path: {Path}",
                           context.Exception.Message,
                           context.Request.Path
                       );

                       return Task.CompletedTask;
                   },

                   OnChallenge = context =>
                   {
                       var logger = context.HttpContext
                           .RequestServices
                           .GetRequiredService<ILoggerFactory>()
                           .CreateLogger("JWT");

                       logger.LogWarning(
                           "Unauthorized request to {Path} | IP: {IP}",
                           context.Request.Path,
                           context.HttpContext.Connection.RemoteIpAddress
                       );

                       return Task.CompletedTask;
                   },

                   OnTokenValidated = context =>
                   {
                       var logger = context.HttpContext
                           .RequestServices
                           .GetRequiredService<ILoggerFactory>()
                           .CreateLogger("JWT");

                       var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                       logger.LogInformation(
                           "Token validated successfully for UserId: {UserId}",
                           userId
                       );

                       return Task.CompletedTask;
                   }
               };
           });
            return services;

        }
    }
}