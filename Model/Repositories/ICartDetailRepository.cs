using Model.Domain;

namespace Model.Repositories
{
    public interface ICartDetailRepository : IRepository<CartDetail, long>
    {
        IQueryable<CartDetail> GetByUserIdAndProductId(long userId, long productId);
    }
}
