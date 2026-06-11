using Catalog.API.Configurations;
using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Common.Settings;
using Catalog.Infrastructure.Persistence.DbContext;
using Catalog.Infrastructure.Persistence.Seeder;
using Common.Logging;

namespace Catalog.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LoggingConfiguration.ConfigureBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureSerilog();

            builder.Services.AddControllers();

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
                    await CategorySeeder.SeedAsync(context.Categories);
                    await CatalogSeeder.SeedAsync(context.Products);
                }
            }
            app.UseCustomRequestLogging();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
