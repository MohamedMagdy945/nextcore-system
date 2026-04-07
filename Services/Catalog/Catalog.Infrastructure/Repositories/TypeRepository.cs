using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {

        private readonly ICatalogDbContext _context;
        public TypeRepository(ICatalogDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductType>> GetAllAsync()
        {
            return await _context.Types.Find(p => true).ToListAsync();
        }
    }
}
