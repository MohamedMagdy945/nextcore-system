using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence.DbContext
{
    public interface IAppDbContext
    {
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<Brand> Brands { get; }
        public IMongoCollection<Category> Categories { get; }
    }
}
