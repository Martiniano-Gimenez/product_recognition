using Service.Data;

namespace Service.ServiceContracts
{
    public interface IGroupService
    {
        Task<List<KeyValueData>> GetAllSelectable();
    }
}
