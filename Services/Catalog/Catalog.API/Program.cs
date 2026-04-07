
namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Catalog API",
                        Description = "An ASP.NET Core Web API for managing catalog micro-services in commerce application",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Mohamed Magdy Said",
                            Email = "mohamedmagdy000022@gmail.com",
                        }
                    });
            }
            );

            builder.Services.AddOpenApi();


            var app = builder.Build();

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
