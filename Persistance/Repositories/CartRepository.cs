using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class CartRepository : EntityFrameworkRepository<Cart, long>, ICartRepository
    {
        public CartRepository(ShoppingCartContext context) : base(context) { }

        public IQueryable<Cart> GetByUserId(long userId)
        {
            return DbSet.AsNoTracking().Where(c => c.IsActive && c.UserId == userId);
        }
    }
}
