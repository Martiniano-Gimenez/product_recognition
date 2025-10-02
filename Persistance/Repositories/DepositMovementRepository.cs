using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class DepositMovementRepository : EntityFrameworkRepository<DepositMovement, long>, IDepositMovementRepository
    {
        public DepositMovementRepository(ShoppingCartContext context) : base(context) { }

        private static readonly Dictionary<string, Func<IQueryable<DepositMovement>, IQueryable<DepositMovement>>> orderByDictionary = new Dictionary<string, Func<IQueryable<DepositMovement>, IQueryable<DepositMovement>>>
        {
            { string.Empty, x => x.OrderBy(o => o.Id) },
            { "IdASC", x => x.OrderBy(o => o.Id) },
            { "IdDESC", x => x.OrderByDescending(o => o.Id) },
            { "DateASC", x => x.OrderBy(o => o.Date) },
            { "DateDESC", x => x.OrderByDescending(o => o.Date) }
        };

        public IQueryable<DepositMovement> GetFilteredByPage(string filter, string orderBy, string sortDirection, long userId, short roleId)
        {
            var query = DbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x => x.Id.ToString().Contains(filter)
                                     || (x.OriginDepositId.HasValue && x.OriginDeposit.Identifier.Contains(filter))
                                     || (x.DestinationDepositId.HasValue && x.DestinationDeposit.Identifier.Contains(filter))); 

            orderBy = string.Format("{0}{1}", orderBy, sortDirection);
            query = orderByDictionary[orderBy](query);

            return query;
        }
    }
}
