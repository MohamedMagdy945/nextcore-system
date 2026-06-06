using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Entities;
using Catalog.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {

        private readonly IAppDbContext _context;
        public TypeRepository(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Types.Find(p => true).ToListAsync();
        }
    }
}
