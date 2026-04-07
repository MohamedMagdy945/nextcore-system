using Catalog.Application.Interfaces;
using Catalog.Core.Entities;
using Catalog.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence
{
    public class CatalogDbContext : ICatalogDbContext
    {
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ProductBrand> Brands { get; }
        public IMongoCollection<ProductType> Types { get; }

        public CatalogDbContext(IMongoClient client, IOptions<DatabaseSettings> options)
        {

            var database = client.GetDatabase(options.Value.DatabaseName);

            Brands = database.GetCollection<ProductBrand>(options.Value.Brands);
            Types = database.GetCollection<ProductType>(options.Value.Types);
            Products = database.GetCollection<Product>(options.Value.Products);

        }
    }
}
