using Catalog.Core.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(string id);
        Task<IEnumerable<Product>> GetAllByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllByBrandNameAsync(string name);

        Task<Product> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(string id);
    }
}
