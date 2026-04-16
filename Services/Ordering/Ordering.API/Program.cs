using EventBus.Messages.Common;
using MassTransit;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructrue.Data;
using Ordering.Infrastructrue.Extensions;


namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            object value = builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Order API",
                        Description = "An ASP.NET Core Web API for managing ordering micro-services in commerce application",
                        Contact = new OpenApiContact
                        {
                            Name = "Mohamed Magdy",
                            Email = "mohamedmagdy000022@gmail.com",
                        }
                    });
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddInfraService(builder.Configuration);
            builder.Services.AddScoped<BasketOrderingConsumer>();



            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketOrderingConsumer>();
                config.UsingRabbitMq((ct, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                    // provide the queue name with consumer 
                    cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketOrderingConsumer>(ct);
                    });
                });
            });

            builder.Services.AddMassTransitHostedService();


            var app = builder.Build();
            app.MigerateDatabast<OrderContext>(
                (context, services) =>
            {
                var logger = services.GetRequiredService<ILogger<OrderContextSeed>>();
                OrderContextSeed.SeedAsync(context, logger).Wait();
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();



            app.MapControllers();



            app.Run();
        }
    }
}
