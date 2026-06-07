using ApiGateway.AuthenticationConfig;
using ApiGateway.Middlewares;
using ApiGateway.Service;
using Common.Logging;
using Microsoft.AspNetCore.Authorization;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingConfiguration.ConfigureBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.ConfigureSerilog();

            // Add services to the container.
            builder.Configuration
                .AddJsonFile("Yarp/yarp.json", optional: false, reloadOnChange: true);


            builder.Services
            .AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization();

            builder.Services.AddSingleton<IAuthorizationHandler,
                PermissionAuthorizationHandler>();

            builder.Services.AddSingleton<IAuthorizationPolicyProvider,
                PermissionPolicyProvider>();

            builder.Services.AddJwtRegistrationService(builder.Configuration);

            var app = builder.Build();

            app.UseCors("AllowAngularApp");


            // Configure the HTTP request pipeline.

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseCustomRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();





            app.MapGet("/", () =>
            {
                return Results.Ok(new
                {
                    Service = "Auth Service",
                    Status = "Working",
                    Time = DateTime.UtcNow
                });
            });

            app.MapReverseProxy();

            app.Run();
        }
    }
}
