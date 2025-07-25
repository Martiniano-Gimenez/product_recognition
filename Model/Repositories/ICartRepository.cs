using Model.Domain;

namespace Model.Repositories
{
    public interface ICartRepository : IRepository<Cart, long>
    {
        IQueryable<Cart> GetByUserId(long userId);
    }
}
