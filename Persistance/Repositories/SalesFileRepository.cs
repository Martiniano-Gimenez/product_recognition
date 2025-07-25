using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class SalesFileRepository : EntityFrameworkRepository<SalesFile, long>, ISalesFileRepository
    {
        public SalesFileRepository(ShoppingCartContext context) : base(context) { }

        private static readonly Dictionary<string, Func<IQueryable<SalesFile>, IQueryable<SalesFile>>> orderByDictionary = new Dictionary<string, Func<IQueryable<SalesFile>, IQueryable<SalesFile>>>
        {
            { string.Empty, x => x.OrderBy(o => o.Name) },
            { "OrderASC", x => x.OrderBy(o => o.Order).ThenBy(o => o.Id) },
            { "OrderDESC", x => x.OrderByDescending(o => o.Order).ThenByDescending(o => o.Id) },
            { "NameASC", x => x.OrderBy(o => o.Name) },
            { "NameDESC", x => x.OrderByDescending(o => o.Name) }
        };

        public IQueryable<SalesFile> GetFilteredByPage(string filter, string orderBy, string sortDirection)
        {
            var query = DbSet.AsNoTracking().Where(sp => sp.IsActive);

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x => x.Name.Contains(filter));

            orderBy = string.Format("{0}{1}", orderBy, sortDirection);
            query = orderByDictionary[orderBy](query);

            return query;
        }
    }
}
