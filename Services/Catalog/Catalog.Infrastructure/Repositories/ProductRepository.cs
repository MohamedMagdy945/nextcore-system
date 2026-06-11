using Catalog.Application.Interfaces.Repositories;
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

        // ================= GET BY ID =================
        public async Task<Product> GetByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId)) return null!;

            return await _context.Products.Aggregate()
                .Match(p => p.Id == id)
                .Lookup<Product, Brand, Product>(_context.Brands, p => p.BrandId, b => b.Id, p => p.Brand)
                .Lookup<Product, Category, Product>(_context.Categories, p => p.CategoryId, c => c.Id, p => p.Category)
                .Unwind<Product, Product>(p => p.Brand, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true })
                .Unwind<Product, Product>(p => p.Category, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true })
                .FirstOrDefaultAsync();
        }

        // ================= GET ALL (PAGINATION) =================
        public async Task<Pagination<Product>> GetAllAsync(ProductParams productParams)
        {
            var filter = BuildFilter(productParams);

            var totalItems = await _context.Products.CountDocumentsAsync(filter);

            var data = await ApplySortAndPagingAsync(productParams, filter);

            return new Pagination<Product>(
                productParams.PageIndex,
                productParams.PageSize,
                (int)totalItems,
                data
            );
        }

        // ================= GET ALL BY NAME =================
        // ================= GET ALL BY NAME (With Brand & Category) =================
        public async Task<IEnumerable<Product>> GetAllByNameAsync(string name)
        {
            var filter = _filterBuilder.Regex(
                p => p.Name,
                new BsonRegularExpression(name, "i")
            );

            return await _context.Products.Aggregate()
                .Match(filter)

                .Lookup<Product, Brand, Product>(
                    _context.Brands,
                    p => p.BrandId,
                    b => b.Id,
                    p => p.Brand
                )
                .Lookup<Product, Category, Product>(
                    _context.Categories,
                    p => p.CategoryId,
                    c => c.Id,
                    p => p.Category
                )
                .Unwind<Product, Product>(p => p.Brand, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true })
                .Unwind<Product, Product>(p => p.Category, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true })
                .ToListAsync();
        }

        // ================= GET ALL BY BRAND NAME =================
        public async Task<IEnumerable<Product>> GetAllByBrandNameAsync(string brandName)
        {
            var brandFilter = Builders<Brand>.Filter.Regex(b => b.Name, new BsonRegularExpression(brandName, "i"));

            var matchedBrands = await _context.Brands.Find(brandFilter).Project(b => b.Id).ToListAsync();

            var filter = _filterBuilder.In(p => p.BrandId, matchedBrands);

            return await _context.Products.Find(filter).ToListAsync();
        }

        // ================= CREATE =================
        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        // ================= UPDATE =================
        public async Task<bool> UpdateAsync(Product product)
        {
            var result = await _context.Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // ================= DELETE =================
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _context.Products
                .DeleteOneAsync(p => p.Id == id);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        // ================= BUILD FILTER (PRIVATE) =================
        private FilterDefinition<Product> BuildFilter(ProductParams productParams)
        {
            var filters = new List<FilterDefinition<Product>>();

            if (!string.IsNullOrWhiteSpace(productParams.Search))
            {
                filters.Add(_filterBuilder.Regex(p => p.Name, new BsonRegularExpression(productParams.Search, "i")));
            }

            if (!string.IsNullOrWhiteSpace(productParams.BrandId))
            {
                filters.Add(_filterBuilder.Eq(p => p.BrandId, productParams.BrandId));
            }

            if (!string.IsNullOrWhiteSpace(productParams.CategoryId))
            {
                filters.Add(_filterBuilder.Eq(p => p.CategoryId, productParams.CategoryId));
            }

            return filters.Count > 0 ? _filterBuilder.And(filters) : _filterBuilder.Empty;
        }

        // ================= SORT & PAGE (PRIVATE) =================
        private async Task<IReadOnlyList<Product>> ApplySortAndPagingAsync(
            ProductParams productParams,
            FilterDefinition<Product> filter)
        {
            var sortOrder = productParams.Sort?.ToLowerInvariant();

            var sort = sortOrder switch
            {
                "pricedesc" => _sortBuilder.Descending(p => p.Price),
                "priceasc" => _sortBuilder.Ascending(p => p.Price),
                _ => _sortBuilder.Ascending(p => p.Name)
            };

            var skip = productParams.PageSize * (productParams.PageIndex - 1);

            return await _context.Products.Aggregate()
                .Match(filter)
                .Sort(sort)
                .Skip(skip)
                .Limit(productParams.PageSize)
                .Lookup<Product, Brand, Product>(_context.Brands, p => p.BrandId, b => b.Id, p => p.Brand)
                .Lookup<Product, Category, Product>(_context.Categories, p => p.CategoryId, c => c.Id, p => p.Category)
                .Unwind<Product, Product>(p => p.Brand, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true })
                .Unwind<Product, Product>(p => p.Category, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true })
                .ToListAsync();
        }
    }
}