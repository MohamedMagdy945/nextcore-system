using Catalog.API.Configurations;
using Catalog.Application.Common;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Infrastructure.Common.Settings;
using Catalog.Infrastructure.Data.Seed;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using MongoDB.Driver;
using Serilog;

namespace Catalog.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Host.UseSerilog(Logging.ConfigureLogger);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly);
            });

            builder.Services.Configure<DatabaseSettings>(
                builder.Configuration.GetSection("DatabaseSettings"));

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(typeof(ApplicationAssemblyMarker).Assembly));

            var databaseSettings = builder.Configuration
                .GetSection("DatabaseSettings")
                .Get<DatabaseSettings>();

            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new MongoClient(databaseSettings?.ConnectionString);
            });

            builder.Services.AddScoped<ICatalogDbContext, CatalogDbContext>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ITypeRepository, TypeRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ICatalogDbContext>();

                await BrandSeeder.SeedAsync(context.Brands);
                await TypeSeeder.SeedAsync(context.Types);
                await CatalogSeeder.SeedAsync(context.Products);
            }

            // Configure the HTTP request pipeline.
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
