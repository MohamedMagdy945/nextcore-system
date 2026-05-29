using Auth.API.Authorization;
using Auth.API.Configurations;
using Auth.API.Middlewares;
using Auth.Application;
using Auth.Infrastructure;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Persistence.DatabaseSeeder;
using Common.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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


            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

                Console.WriteLine("Applying migrations...");

                db.Database.Migrate();

                Console.WriteLine("Migration completed");
            }


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
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}