
using AppCoreSystem.API.Configurations;
using Basket.Application;
using Basket.Infrastructure;
using Common.Logging;
using MassTransit;
using Serilog;
namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Host.UseSerilog(Logging.ConfigureLogger);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddApiVersioningConfiguration();

            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ct, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });
            });

            builder.Services.AddMassTransitHostedService();

            builder.Services.AddApplicationService();

            builder.Services.AddInfrastructureService(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
