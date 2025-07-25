using Model.Domain;

namespace Model.Repositories
{
    public interface ISalesFileRepository : IRepository<SalesFile, long>
    {
        IQueryable<SalesFile> GetFilteredByPage(string filter, string orderBy, string sortDirection);
    }
}
