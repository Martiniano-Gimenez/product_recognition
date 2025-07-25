using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class OrderRepository : EntityFrameworkRepository<Order, long>, IOrderRepository
    {
        public OrderRepository(ShoppingCartContext context) : base(context) { }

        private static readonly Dictionary<string, Func<IQueryable<Order>, IQueryable<Order>>> orderByDictionary = new Dictionary<string, Func<IQueryable<Order>, IQueryable<Order>>>
        {
            { string.Empty, x => x.OrderBy(o => o.Id) },
            { "IdASC", x => x.OrderBy(o => o.Id) },
            { "IdDESC", x => x.OrderByDescending(o => o.Id) },
            { "DateASC", x => x.OrderBy(o => o.Date) },
            { "DateDESC", x => x.OrderByDescending(o => o.Date) },
            { "TotalASC", x => x.OrderBy(o => o.Total) },
            { "TotalDESC", x => x.OrderByDescending(o => o.Total) },
            { "ClientASC", x => x.OrderBy(o => o.Client.Name) },
            { "ClientDESC", x => x.OrderByDescending(o => o.Client.Name) },
            { "SellerASC", x => x.OrderBy(o => o.SellerId.HasValue ? o.Seller.Name : string.Empty) },
            { "SellerDESC", x => x.OrderByDescending(o => o.SellerId.HasValue ? o.Seller.Name : string.Empty) },
            { "StateClassASC", x => x.OrderBy(o => o.OrderState) },
            { "StateClassDESC", x => x.OrderByDescending(o => o.OrderState) },
        };

        public IQueryable<Order> GetFilteredByPageForShoppingCart(string filter, string orderBy, string sortDirection, long userId)
        {
            var query = DbSet.AsNoTracking().Where(sp => sp.Client.Users.Any(u => u.Id == userId));

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x => x.Id.ToString().Contains(filter)
                                    || x.Client.Name.Contains(filter));

            orderBy = string.Format("{0}{1}", orderBy, sortDirection);
            query = orderByDictionary[orderBy](query);

            return query;
        }

        public IQueryable<Order> GetFilteredByPage(string filter, string orderBy, string sortDirection, long userId, short roleId)
        {
            var query = DbSet.AsNoTracking();

            if(roleId == (short)eRole.StockManager)
                query = query.Where(sp => sp.Seller.Users.Any(u => u.Id == userId));

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x => x.Id.ToString().Contains(filter)
                                    || x.Client.Name.Contains(filter)
                                    || (x.SellerId.HasValue && x.Seller.Name.Contains(filter))); 

            orderBy = string.Format("{0}{1}", orderBy, sortDirection);
            query = orderByDictionary[orderBy](query);

            return query;
        }
    }
}
