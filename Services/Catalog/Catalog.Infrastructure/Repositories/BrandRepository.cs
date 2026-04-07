using Catalog.Application.Interfaces;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {

        private readonly ICatalogDbContext _context;
        public BrandRepository(ICatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllAsync()
        {
            return await _context.Brands.Find(p => true).ToListAsync();
        }
    }
}
