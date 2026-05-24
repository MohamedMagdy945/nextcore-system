using Auth.API.Configurations;
using Auth.API.Middlewares;
using Auth.Application;
using Auth.Infrastructure;
using Common.Logging;
namespace Auth.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingConfiguration.ConfigureBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureSerilog();

            builder.Services.AddCustomLocalization();

            builder.Services.AddApiVersioningConfiguration();

            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddApplicationServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
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