using Service.Data;

namespace Service.ServiceContracts
{
    public interface IUserService
    {
        Task<UserData?> Login(LoginData data);
        Task<bool> ChangePassword(ChangePasswordData data);
        Task Create(string userName, string password, long? clientId, long? sellerId);
        Task Edit(string oldUserName, string newUserName);
        Task<GridData<UserGridData>> GetAllPaginated(DTParameters param);
        Task<bool> ResetPassword(long userId);
    }
}
