using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class ClientRepository : EntityFrameworkRepository<Client, long>, IClientRepository
    {
        public ClientRepository(ShoppingCartContext context) : base(context) { }

        private static readonly Dictionary<string, Func<IQueryable<Client>, IQueryable<Client>>> orderByDictionary = new Dictionary<string, Func<IQueryable<Client>, IQueryable<Client>>>
        {
            { string.Empty, x => x.OrderBy(o => o.Name) },
            { "NameASC", x => x.OrderBy(o => o.Name) },
            { "NameDESC", x => x.OrderByDescending(o => o.Name) },
            { "CUILASC", x => x.OrderBy(o => o.CUIL) },
            { "CUILDESC", x => x.OrderByDescending(o => o.CUIL) },
            { "EmailASC", x => x.OrderBy(o => o.Email) },
            { "EmailDESC", x => x.OrderByDescending(o => o.Email) },
            { "CellPhoneASC", x => x.OrderBy(o => o.CellPhone) },
            { "CellPhoneDESC", x => x.OrderByDescending(o => o.CellPhone) },
            { "TelephoneASC", x => x.OrderBy(o => o.Telephone) },
            { "TelephoneDESC", x => x.OrderByDescending(o => o.Telephone) },
        };

        public IQueryable<Client> GetFilteredByPage(string filter, string orderBy, string sortDirection)
        {
            var query = DbSet.AsNoTracking().Where(sp => sp.IsActive);

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x => x.Name.Contains(filter) 
                                      || x.CUIL.Contains(filter)); 

            orderBy = string.Format("{0}{1}", orderBy, sortDirection);
            query = orderByDictionary[orderBy](query);

            return query;
        }

        public IQueryable<Client> GetBySellerId(long sellerId)
        {
            return DbSet.AsNoTracking();
        }

        public async Task<bool> ExistsAnyWithCuil(string cuil, long? id = null)
        {
            return !id.HasValue ? await DbSet.AnyAsync(f => f.IsActive && f.CUIL == cuil) 
                : await DbSet.AnyAsync(f => f.IsActive && f.Id != id && f.CUIL == cuil);
        }
    }
}
