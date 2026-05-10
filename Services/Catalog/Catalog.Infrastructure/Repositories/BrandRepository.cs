using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Entities;
using Catalog.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {

        private readonly IAppDbContext _context;
        public BrandRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllAsync()
        {
            return await _context.Brands.Find(p => true).ToListAsync();
        }
    }
}
