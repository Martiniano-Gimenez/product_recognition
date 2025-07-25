using Service.Data;

namespace Service.ServiceContracts
{
    public interface ICategoryService
    {
        Task<List<KeyValueData>> GetAllSelectable();
    }
}
