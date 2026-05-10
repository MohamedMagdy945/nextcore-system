using Catalog.Core.Entities;
using Catalog.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence.DbContext
{
    public class AppDbContext : IAppDbContext
    {
        public AppDbContext(IMongoClient client, IOptions<DatabaseSettings> options)
        {

            var database = client.GetDatabase(options.Value.DatabaseName);

            Brands = database.GetCollection<ProductBrand>(options.Value.Brands);
            Types = database.GetCollection<ProductType>(options.Value.Types);
            Products = database.GetCollection<Product>(options.Value.Products);
        }
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ProductBrand> Brands { get; }
        public IMongoCollection<ProductType> Types { get; }
    }
}
