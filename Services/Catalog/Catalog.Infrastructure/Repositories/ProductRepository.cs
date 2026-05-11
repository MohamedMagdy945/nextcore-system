using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Entities;
using Catalog.Core.Specs;
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

        public async Task<Pagination<Product>> GetAllAsync(CatalogSpecParams spec)
        {
            var filter = BuildFilter(spec);

            var totalItems = await _context.Products
                .CountDocumentsAsync(filter);

            var data = await ApplySortAndPagingAsync(spec, filter);

            return new Pagination<Product>(
                spec.PageIndex,
                spec.PageSize,
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

        private FilterDefinition<Product> BuildFilter(CatalogSpecParams spec)
        {
            var filters = new List<FilterDefinition<Product>>();

            if (!string.IsNullOrWhiteSpace(spec.Search))
            {
                filters.Add(
                    _filterBuilder.Regex(
                        p => p.Name,
                        new BsonRegularExpression(spec.Search, "i")
                    )
                );
            }

            if (!string.IsNullOrWhiteSpace(spec.BrandId))
            {
                filters.Add(
                    _filterBuilder.Eq(p => p.Brand.Id, spec.BrandId)
                );
            }

            if (!string.IsNullOrWhiteSpace(spec.TypeId))
            {
                filters.Add(
                    _filterBuilder.Eq(p => p.Type.Id, spec.TypeId)
                );
            }

            return filters.Count > 0
                ? _filterBuilder.And(filters)
                : _filterBuilder.Empty;
        }

        private async Task<IReadOnlyList<Product>> ApplySortAndPagingAsync(
            CatalogSpecParams spec,
            FilterDefinition<Product> filter)
        {
            var sort = (spec.Sort ?? "").ToLowerInvariant() switch
            {
                "priceasc" => _sortBuilder.Ascending(p => p.Price),
                "pricedesc" => _sortBuilder.Descending(p => p.Price),
                _ => _sortBuilder.Ascending(p => p.Name)
            };

            var skip = spec.PageSize * (spec.PageIndex - 1);

            return await _context.Products
                .Find(filter)
                .Sort(sort)
                .Skip(skip)
                .Limit(spec.PageSize)
                .ToListAsync();
        }
    }
}