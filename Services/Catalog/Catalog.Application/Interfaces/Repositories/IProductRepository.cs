using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Pagination<Product>> GetAllAsync(CatalogSpecParams catalogSpecParams);
        Task<Product> GetByIdAsync(string id);
        Task<IEnumerable<Product>> GetAllByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllByBrandNameAsync(string name);

        Task<Product> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(string id);
    }
}
