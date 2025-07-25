using Model.Domain;

namespace Model.Repositories
{
    public interface IProductRepository : IRepository<Product, long>
    {
        IQueryable<Product> GetFilteredByPage(string filter, string orderBy, string sortDirection, long? groupId, long? categoryId);
        IQueryable<Product> GetByIds(List<long> ids);
        IQueryable<Product> GetByTerm(string term);
    }
}
