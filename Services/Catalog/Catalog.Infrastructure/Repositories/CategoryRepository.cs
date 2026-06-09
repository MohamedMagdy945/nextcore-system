using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Entities;
using Catalog.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly IAppDbContext _context;
        public CategoryRepository(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Find(p => true).ToListAsync();
        }
    }
}
