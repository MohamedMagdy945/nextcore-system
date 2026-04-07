using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(string id);
        Task<IEnumerable<Product>> GetAllByName(string name);
        Task<IEnumerable<Product>> GetAllByBrand(string name);
        Task<Product> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task DeleteAsync(string id);
    }
}
