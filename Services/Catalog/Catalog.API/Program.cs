using Catalog.Application.Mappers;
using Catalog.Infrastructure.Common.Settings;
using Catalog.Infrastructure.Data.Seed;
using Catalog.Infrastructure.Persistence;

namespace Catalog.API
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                            Name = "Mohamed Magdy",
                            Email = "mohamedmagdy000022@gmail.com",
                        }
                    });
            }
            );

            builder.Services.AddOpenApi();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ProductMappingProfile).Assembly);
            });

            builder.Services.Configure<DatabaseSettings>(
                builder.Configuration.GetSection("DatabaseSettings"));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                await BrandSeeder.SeedAsync(context.Brands);
                await TypeSeeder.SeedAsync(context.Types);
                await CatalogSeeder.SeedAsync(context.Products);
            }

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
