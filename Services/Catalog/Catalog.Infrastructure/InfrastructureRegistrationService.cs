using Catalog.Application.Interfaces.Repositories;
using Catalog.Infrastructure.Common.Settings;
using Catalog.Infrastructure.Persistence.DbContext;
using Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace Catalog.Infrastructure
{
    public static class InfrastructureRegistrationService
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {

            // Register Mongo client using the configured settings
            services.AddSingleton<IMongoClient>(sp =>
            {
                var dbSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                return new MongoClient(dbSettings.ConnectionString);
            });

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();

            return services;
        }
    }
}
