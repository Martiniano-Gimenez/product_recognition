using Model.Domain;

namespace Model.Repositories
{
    public interface IClientRepository : IRepository<Client, long>
    {
        IQueryable<Client> GetFilteredByPage(string filter, string orderBy, string sortDirection);
        IQueryable<Client> GetBySellerId(long sellerId);
        Task<bool> ExistsAnyWithCuil(string cuil, long? id = null);
    }
}
