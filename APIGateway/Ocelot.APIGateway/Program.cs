using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Ocelot.APIGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Configuration
                .AddJsonFile(
                    $"ocelot.{builder.Environment.EnvironmentName}.json",
                    optional: false,
                    reloadOnChange: true);

            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    await context.Response.WriteAsync("Gateway Running");
                    return;
                }

                await next();
            });

            await app.UseOcelot();

            await app.RunAsync();
        }
    }
}
