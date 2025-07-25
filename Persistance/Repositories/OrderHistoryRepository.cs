using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class OrderHistoryRepository : EntityFrameworkRepository<OrderHistory, long>, IOrderHistoryRepository
    {
        public OrderHistoryRepository(ShoppingCartContext context) : base(context) { }

        public IQueryable<OrderHistory> GetByOrderId(long orderId)
        {
            return DbSet.AsNoTracking().Where(oh => oh.OrderId == orderId);
        }
    }
}
