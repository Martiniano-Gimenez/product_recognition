using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class ProductRepository : EntityFrameworkRepository<Product, long>, IProductRepository
    {
        public ProductRepository(ShoppingCartContext context) : base(context) { }

        private static readonly Dictionary<string, Func<IQueryable<Product>, IQueryable<Product>>> orderByDictionary = new Dictionary<string, Func<IQueryable<Product>, IQueryable<Product>>>
        {
            { string.Empty, x => x.OrderBy(o => o.Code) },
            { "CodeASC", x => x.OrderBy(o => o.Code) },
            { "CodeDESC", x => x.OrderByDescending(o => o.Code) },
            { "NameASC", x => x.OrderBy(o => o.Name) },
            { "NameDESC", x => x.OrderByDescending(o => o.Name) },
            { "PriceASC", x => x.OrderBy(o => o.SalePrice) },
            { "PriceDESC", x => x.OrderByDescending(o => o.SalePrice) },
            { "IvaPercentageASC", x => x.OrderBy(o => o.IvaPercentage) },
            { "IvaPercentageDESC", x => x.OrderByDescending(o => o.IvaPercentage) },
        };

        public IQueryable<Product> GetFilteredByPage(string filter, string orderBy, string sortDirection, long? groupId, long? categoryId)
        {
            var query = DbSet.AsNoTracking().Where(sp => sp.IsActive);

            if(categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x => x.Code.Contains(filter)
                                    || x.Name.Contains(filter)
                                    || x.Description.Contains(filter)); 

            orderBy = string.Format("{0}{1}", orderBy, sortDirection);
            query = orderByDictionary[orderBy](query);

            return query;
        }

        public IQueryable<Product> GetByIds(List<long> ids)
        {
            return DbSet.AsNoTracking().Where(p => p.IsActive && ids.Contains(p.Id));
        }

        public IQueryable<Product> GetByTerm(string term)
        {
            return DbSet.AsNoTracking().Where(p => p.IsActive && (p.Code.Contains(term) || p.Name.Contains(term)
                                                                    || (!string.IsNullOrWhiteSpace(p.Description) && p.Description.Contains(term))));
        }

        public async Task<bool> ExistCode(string code, long? ignoreId = null)
        {
            return await DbSet.AnyAsync(p => p.Id != ignoreId && p.IsActive && p.Code == code);
        }
    }
}
