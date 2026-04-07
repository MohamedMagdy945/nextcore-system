using Catalog.Core.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<ProductBrand>> GetAllAsync();
    }
}
