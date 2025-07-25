using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class CartDetailRepository : EntityFrameworkRepository<CartDetail, long>, ICartDetailRepository
    {
        public CartDetailRepository(ShoppingCartContext context) : base(context) { }

        public IQueryable<CartDetail> GetByUserIdAndProductId(long userId, long productId)
        {
            return DbSet.Where(cc => cc.Cart.UserId == userId && cc.ProductId == productId);
        }
    }
}
