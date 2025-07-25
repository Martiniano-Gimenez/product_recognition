using Model.Domain;

namespace Model.Repositories
{
    public interface IOrderHistoryRepository : IRepository<OrderHistory, long>
    {
        IQueryable<OrderHistory> GetByOrderId(long orderId);
    }
}
