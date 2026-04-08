using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Entities;
using Catalog.Core.Specs;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogDbContext _context;
        public ProductRepository(ICatalogDbContext context)
        {
            _context = context;
        }
        public async Task<Product> GetByIdAsync(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Pagination<Product>> GetAllAsync(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;


            if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                filter = filter & builder.Where(p => p.Name.ToLower().Contains(catalogSpecParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                var brandFilter = builder.Eq(p => p.Brand.Id, catalogSpecParams.BrandId);
                filter = filter & brandFilter;
            }

            if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                var typeFilter = builder.Eq(p => p.Type.Id, catalogSpecParams.TypeId);
                filter = filter & typeFilter;
            }
            var totalItems = await _context.Products.CountDocumentsAsync(filter);

            var data = await DataFilter(catalogSpecParams, filter);

            var pagination = new Pagination<Product>(
                catalogSpecParams.PageIndex,
                catalogSpecParams.PageSize,
                (int)totalItems,
                data
                );

            return pagination;
        }
        public async Task<IEnumerable<Product>> GetAllByNameAsync(string name)
        {
            return await _context.Products.Find(p => p.Name == name).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllByBrandNameAsync(string name)
        {
            return await _context.Products.Find(p => p.Brand.Name == name).ToListAsync();
        }
        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deletedProduct = await _context.Products.DeleteOneAsync(p => p.Id == id);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var updateProduct = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
        }
        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            var sort = Builders<Product>.Sort.Ascending(p => p.Name);

            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                switch (catalogSpecParams.Sort.ToLower())
                {
                    case "priceAsc":
                        sort = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "priceDesc":
                        sort = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sort = Builders<Product>.Sort.Ascending(p => p.Name);
                        break;
                }
            }

            return await _context.Products.Find(filter)
                .Sort(sort)
                .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync();
        }
    }
}
