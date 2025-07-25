using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class GroupRepository : EntityFrameworkRepository<Group, long>, IGroupRepository
    {
        public GroupRepository(ShoppingCartContext context) : base(context) { }
    }
}
