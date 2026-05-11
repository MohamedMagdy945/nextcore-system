using Catalog.API.Configurations;
using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Common.Settings;
using Catalog.Infrastructure.Persistence.DbContext;
using Catalog.Infrastructure.Persistence.Seeder;
using Common.Logging;
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
            builder.Services.AddApiVersioningConfiguration();
            builder.Services.AddSwaggerConfiguration();


            builder.Services.Configure<DatabaseSettings>(
                builder.Configuration.GetSection("DatabaseSettings"));

            builder.Services.AddApplicationService();
            builder.Services.AddInfrastructureService(builder.Configuration);

            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();

                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

                    await BrandSeeder.SeedAsync(context.Brands);
                    await TypeSeeder.SeedAsync(context.Types);
                    await CatalogSeeder.SeedAsync(context.Products);
                }
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
