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

            Brands = database.GetCollection<Brand>(options.Value.Brands);
            Categories = database.GetCollection<Category>(options.Value.Categories);
            Products = database.GetCollection<Product>(options.Value.Products);
        }
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<Brand> Brands { get; }
        public IMongoCollection<Category> Categories { get; }
    }
}
