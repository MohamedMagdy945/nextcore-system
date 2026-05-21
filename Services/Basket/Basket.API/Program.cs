
using AppCoreSystem.API.Configurations;
using Basket.API.Settings;
using Basket.Application;
using Basket.Infrastructure;
using Common.Logging;
using MassTransit;
using Microsoft.Extensions.Options;
namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingConfiguration.ConfigureBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.ConfigureSerilog();

            builder.Services.AddControllers();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddApiVersioningConfiguration();

            builder.Services.AddSwaggerConfiguration();

            builder.Services.Configure<EventBusSettings>(
                builder.Configuration.GetSection("EventBusSettings"));

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var settings = context.GetRequiredService<IOptions<EventBusSettings>>().Value;

                    cfg.Host(new Uri($"rabbitmq://{settings.Host}:{settings.Port}"), h =>
                    {
                        h.Username(settings.Username);
                        h.Password(settings.Password);
                    });
                });
            });
            //builder.Services.AddMassTransitHostedService();

            builder.Services.AddApplicationService();

            builder.Services.AddInfrastructureService(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }

            app.UseCustomRequestLogging();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
