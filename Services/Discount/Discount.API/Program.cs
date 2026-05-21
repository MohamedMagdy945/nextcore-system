using Common.Logging;
using Discount.API.Services;
using Discount.Application;
using Discount.Infrastructure;
using Discount.Infrastructure.Extensions;

namespace Discount.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LoggingConfiguration.ConfigureBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureSerilog();

            builder.Services.AddApplicationService();
            builder.Services.AddInfrastructureService(builder.Configuration);


            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                await app.MigrateDatabaseAsync<Program>();

            }

            app.UseCustomRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DiscountService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        "Communication with gRPC endpoints must be made through a gRPC client"
                    );
                });
            });

            await app.RunAsync();
        }
    }
}