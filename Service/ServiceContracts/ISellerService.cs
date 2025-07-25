using Service.Data;

namespace Service.ServiceContracts
{
    public interface ISellerService
    {
        Task<List<KeyValueData>> GetAllSelectable();
    }
}
