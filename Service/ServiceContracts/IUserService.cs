using Service.Data;

namespace Service.ServiceContracts
{
    public interface IUserService
    {
        Task<UserData?> Login(LoginData data);
        Task<bool> ChangePassword(ChangePasswordData data);
        Task<bool> Create(UserData data);
        Task<UserData> GetById(long id);
        Task<bool> Edit(UserData data);
        Task<bool> Delete(long id);
        Task<GridData<UserGridData>> GetAllPaginated(DTParameters param);
        Task<bool> ResetPassword(long userId);
        List<KeyValueData> GetAllRoles();
    }
}
