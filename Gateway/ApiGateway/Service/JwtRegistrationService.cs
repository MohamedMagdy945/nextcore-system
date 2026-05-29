using ApiGateway.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Service
{
    public static class JwtRegistrationService
    {
        public static IServiceCollection AddJwtRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));

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
