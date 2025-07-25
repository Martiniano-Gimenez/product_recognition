using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class SellerRepository : EntityFrameworkRepository<Seller, long>, ISellerRepository
    {
        public SellerRepository(ShoppingCartContext context) : base(context) { }
    }
}
