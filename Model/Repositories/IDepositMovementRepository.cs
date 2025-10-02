using Model.Domain;

namespace Model.Repositories
{
    public interface IDepositMovementRepository : IRepository<DepositMovement, long>
    {
        IQueryable<DepositMovement> GetFilteredByPage(string filter, string orderBy, string sortDirection, long userId, short roleId);
    }
}
