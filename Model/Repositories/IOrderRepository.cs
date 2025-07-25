using Model.Domain;

namespace Model.Repositories
{
    public interface IOrderRepository : IRepository<Order, long>
    {
        IQueryable<Order> GetFilteredByPageForShoppingCart(string filter, string orderBy, string sortDirection, long userId);
        IQueryable<Order> GetFilteredByPage(string filter, string orderBy, string sortDirection, long userId, short roleId);
    }
}
