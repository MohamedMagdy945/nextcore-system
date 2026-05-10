using Catalog.Core.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface ITypeRepository
    {
        Task<IEnumerable<ProductType>> GetAllAsync();
    }
}
