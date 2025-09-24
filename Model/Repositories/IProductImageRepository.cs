using Model.Domain;

namespace Model.Repositories
{
    public interface IProductImageRepository : IRepository<ProductImage, long>
    {
        IQueryable<ProductImage> GetFilteredByPage();
    }
}
