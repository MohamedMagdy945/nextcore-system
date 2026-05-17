using Common.Logging;
using Ordering.API.Configurations;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Extensions;
using Serilog;


namespace Ordering.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog(Logging.ConfigureLogger);
            builder.Services.AddApiVersioningConfiguration();
            builder.Services.AddSwaggerConfiguration();


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            //builder.Services.AddScoped<BasketOrderingConsumer>();
            //builder.Services.AddMassTransit(config =>
            //{
            //    config.AddConsumer<BasketOrderingConsumer>();
            //    config.UsingRabbitMq((ct, cfg) =>
            //    {
            //        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
            //        // provide the queue name with consumer 
            //        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
            //        {
            //            c.ConfigureConsumer<BasketOrderingConsumer>(ct);
            //        });
            //    });
            //});

            //builder.Services.AddMassTransitHostedService();



            builder.Services.AddApplicationService();
            builder.Services.AddInfrastructureService(builder.Configuration);

            var app = builder.Build();

            app.Services.ApplyMigrations();
            await app.Services.SeedDatabaseAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
