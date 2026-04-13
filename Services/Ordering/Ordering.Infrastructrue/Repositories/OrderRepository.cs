using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructrue.Data;

namespace Ordering.Infrastructrue.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName)
        {
            var orderList = await _dbContext.Orders.Where(o => o.UserName == userName).ToListAsync();
            return orderList;
        }
    }
}
