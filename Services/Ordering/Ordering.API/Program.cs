using Common.Logging;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.Extensions.Options;
using Ordering.API.Configurations;
using Ordering.API.EventBusConsumer;
using Ordering.API.Settings;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Extensions;


namespace Ordering.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LoggingConfiguration.ConfigureBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureSerilog();

            builder.Services.AddApiVersioningConfiguration();
            builder.Services.AddSwaggerConfiguration();


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            // 1. Options
            builder.Services.Configure<EventBusSettings>(
                builder.Configuration.GetSection("EventBusSettings"));

            // 2. MassTransit
            builder.Services.AddMassTransit(x =>
            {
                // Consumer
                x.AddConsumer<BasketOrderingConsumer>();
                x.AddConsumer<BasketOrderingConsumerV2>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var settings = context.GetRequiredService<IOptions<EventBusSettings>>().Value;

                    cfg.Host(new Uri($"rabbitmq://{settings.Host}:{settings.Port}"), h =>
                    {
                        h.Username(settings.Username);
                        h.Password(settings.Password);
                    });

                    // Receive Endpoint
                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, e =>
                    {
                        e.ConfigureConsumer<BasketOrderingConsumer>(context);
                    });
                });
            });
            builder.Services.AddMassTransitHostedService();



            builder.Services.AddApplicationService();
            builder.Services.AddInfrastructureService(builder.Configuration);

            var app = builder.Build();

            app.Services.ApplyMigrations();
            await app.Services.SeedDatabaseAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }
            app.UseCustomRequestLogging();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
