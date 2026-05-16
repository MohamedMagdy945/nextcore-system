using Common.Logging;
using Discount.API.Services;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using Serilog;

namespace Discount.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog(Logging.ConfigureLogger);

            builder.Services.AddControllers();

            builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                await app.MigrateDatabaseAsync<Program>();

            }


            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

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