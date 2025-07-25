using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class UserRepository : EntityFrameworkRepository<User, long>, IUserRepository
    {
        public UserRepository(ShoppingCartContext context) : base(context) { }

        private static readonly Dictionary<string, Func<IQueryable<User>, IQueryable<User>>> orderByDictionary = new Dictionary<string, Func<IQueryable<User>, IQueryable<User>>>
        {
            { string.Empty, x => x.OrderBy(o => o.UserName) },
            { "UserNameASC", x => x.OrderBy(o => o.UserName) },
            { "UserNameDESC", x => x.OrderByDescending(o => o.UserName) },
            { "RoleASC", x => x.OrderBy(o => o.RoleId) },
            { "RoleDESC", x => x.OrderByDescending(o => o.RoleId) },
            { "NameASC", x => x.OrderBy(o => o.SellerId.HasValue ? o.Seller.CUIL : (o.ClientId.HasValue ? o.Client.CUIL : string.Empty)).ThenBy(o => o.SellerId.HasValue ? o.Seller.Name : (o.ClientId.HasValue ? o.Client.Name : string.Empty)) },
            { "NameDESC", x => x.OrderByDescending(o => o.SellerId.HasValue ? o.Seller.Name : (o.ClientId.HasValue ? o.Client.Name : string.Empty)).ThenByDescending(o => o.SellerId.HasValue ? o.Seller.Name : (o.ClientId.HasValue ? o.Client.Name : string.Empty))  },
        };

        public IQueryable<User> GetFilteredByPage(string filter, string orderBy, string sortDirection)
        {
            var query = DbSet.AsNoTracking().Where(sp => sp.IsActive);

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x => x.UserName.Contains(filter)
                                      || (x.SellerId.HasValue && x.Seller.CUIL.Contains(filter))
                                      || (x.SellerId.HasValue && x.Seller.Name.Contains(filter))
                                      || (x.ClientId.HasValue && x.Seller.CUIL.Contains(filter))
                                      || (x.ClientId.HasValue && x.Client.Name.Contains(filter)));

            orderBy = string.Format("{0}{1}", orderBy, sortDirection);
            query = orderByDictionary[orderBy](query);

            return query;
        }

        public IQueryable<User> Login(string userName, string password)
        {
            return DbSet.Where(f => f.IsActive && f.UserName == userName && f.Password == password);
        }

        public IQueryable<User> ChangePassword(long id, string password)
        {
            return DbSet.Where(f => f.IsActive && f.Id == id && f.Password == password);
        }

        public async Task<bool> ExistsAnyWithUserName(string userName)
        {
            return await DbSet.AnyAsync(f => f.IsActive && f.UserName == userName);
        }
    }
}
