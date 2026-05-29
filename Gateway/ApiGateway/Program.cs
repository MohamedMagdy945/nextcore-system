namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration
                .AddJsonFile("Yarp/yarp.json", optional: false, reloadOnChange: true);


            builder.Services
            .AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));



            var app = builder.Build();


            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.MapGet("/", () =>
            {
                return Results.Ok(new
                {
                    Service = "Auth Service",
                    Status = "Working",
                    Time = DateTime.UtcNow
                });
            });

            app.MapReverseProxy();

            app.Run();
        }
    }
}
