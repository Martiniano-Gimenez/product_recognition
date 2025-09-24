using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class ProductImageRepository : EntityFrameworkRepository<ProductImage, long>, IProductImageRepository
    {
        public ProductImageRepository(ShoppingCartContext context) : base(context) { }

        public IQueryable<ProductImage> GetFilteredByPage()
        {
            return DbSet.AsNoTracking().Where(sp => sp.IsActive);
        }
    }
}
