using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Catalog.Core.Models;
using Catalog.Infrastructure.Persistence.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IAppDbContext _context;
        private readonly FilterDefinitionBuilder<Product> _filterBuilder = Builders<Product>.Filter;
        private readonly SortDefinitionBuilder<Product> _sortBuilder = Builders<Product>.Sort;

        public ProductRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _context.Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Pagination<Product>> GetAllAsync(ProductParams productParams)
        {
            var filter = BuildFilter(productParams);

            var totalItems = await _context.Products
                .CountDocumentsAsync(filter);

            var data = await ApplySortAndPagingAsync(productParams, filter);

            return new Pagination<Product>(
                productParams.PageIndex,
                productParams.PageSize,
                (int)totalItems,
                data
            );
        }

        public async Task<IEnumerable<Product>> GetAllByNameAsync(string name)
        {
            var filter = _filterBuilder.Regex(
                p => p.Name,
                new BsonRegularExpression(name, "i")
            );

            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByBrandNameAsync(string name)
        {
            var filter = _filterBuilder.Regex(
                p => p.Brand.Name,
                new BsonRegularExpression(name, "i")
            );

            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var result = await _context.Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _context.Products
                .DeleteOneAsync(p => p.Id == id);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        private FilterDefinition<Product> BuildFilter(ProductParams productParams)
        {
            var filters = new List<FilterDefinition<Product>>();

            if (!string.IsNullOrWhiteSpace(productParams.Search))
            {
                filters.Add(
                    _filterBuilder.Regex(
                        p => p.Name,
                        new BsonRegularExpression(productParams.Search, "i")
                    )
                );
            }

            if (!string.IsNullOrWhiteSpace(productParams.BrandId))
            {
                filters.Add(
                    _filterBuilder.Eq(p => p.Brand.Id, productParams.BrandId)
                );
            }

            if (!string.IsNullOrWhiteSpace(productParams.TypeId))
            {
                filters.Add(
                    _filterBuilder.Eq(p => p.Type.Id, productParams.TypeId)
                );
            }

            return filters.Count > 0
                ? _filterBuilder.And(filters)
                : _filterBuilder.Empty;
        }

        private async Task<IReadOnlyList<Product>> ApplySortAndPagingAsync(
            ProductParams productParams,
            FilterDefinition<Product> filter)
        {
            var sort = (productParams.Sort ?? "").ToLowerInvariant() switch
            {
                ProductSortOptions.PriceDesc => _sortBuilder.Ascending(p => p.Price),
                ProductSortOptions.PriceAsc => _sortBuilder.Descending(p => p.Price),
                _ => _sortBuilder.Ascending(p => p.Name)
            };

            var skip = productParams.PageSize * (productParams.PageIndex - 1);

            return await _context.Products
                .Find(filter)
                .Sort(sort)
                .Skip(skip)
                .Limit(productParams.PageSize)
                .ToListAsync();
        }
    }
}