using Auth.API.Authorization;
using Auth.API.Configurations;
using Auth.API.Middlewares;
using Auth.Application;
using Auth.Infrastructure;
using Auth.Infrastructure.Persistence.DatabaseSeeder;
using Common.Logging;
using Microsoft.AspNetCore.Authorization;
namespace Auth.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LoggingConfiguration.ConfigureBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureSerilog();

            builder.Services.AddCustomLocalization();

            builder.Services.AddApiVersioningConfiguration();

            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization();

            builder.Services.AddSingleton<IAuthorizationHandler,
                PermissionAuthorizationHandler>();

            builder.Services.AddSingleton<IAuthorizationPolicyProvider,
                PermissionPolicyProvider>();

            builder.Services.AddApplicationServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
                using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
                    await seeder.SeedAsync();
                }
            }


            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseCustomRequestLogging();

            app.UseCustomLocalization();

            //app.UseHttpsRedirection(); 
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}