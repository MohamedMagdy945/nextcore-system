using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName)
        {
            return await _dbContext.Set<Order>()
                .AsNoTracking()
                .Where(o => o.UserName == userName)
                .ToListAsync();
        }
    }
}