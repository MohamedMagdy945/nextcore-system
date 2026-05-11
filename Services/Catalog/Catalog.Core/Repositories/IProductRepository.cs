using Catalog.Core.Entities;
using Catalog.Core.Models;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Pagination<Product>> GetAllAsync(ProductParams productParams);
        Task<Product> GetByIdAsync(string id);
        Task<IEnumerable<Product>> GetAllByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllByBrandNameAsync(string name);

        Task<Product> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(string id);
    }
}
