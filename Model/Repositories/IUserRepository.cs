using Model.Domain;

namespace Model.Repositories
{
    public interface IUserRepository : IRepository<User, long>
    {
        IQueryable<User> GetFilteredByPage(string filter, string orderBy, string sortDirection);
        IQueryable<User> Login(string userName, string password);
        IQueryable<User> ChangePassword(long id, string password);
        Task<bool> ExistsAnyWithUserName(string userName);
    }
}
