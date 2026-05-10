using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence.DbContext
{
    public interface IAppDbContext
    {
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ProductBrand> Brands { get; }
        public IMongoCollection<ProductType> Types { get; }
    }
}
