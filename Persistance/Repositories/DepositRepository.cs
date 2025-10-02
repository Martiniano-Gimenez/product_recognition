using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class DepositRepository : EntityFrameworkRepository<Deposit, long>, IDepositRepository
    {
        public DepositRepository(ShoppingCartContext context) : base(context) { }
    }
}
