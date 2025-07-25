using Service.Data;

namespace Service.ServiceContracts
{
    public interface IClientService
    {
        Task<GridData<ClientGridData>> GetAllPaginated(DTParameters param);
        Task<bool> Create(ClientData data);
        Task<ClientData> GetById(long id);
        Task<bool> Edit(ClientData data);
        Task<ClientViewData> GetViewById(long id);
        Task<List<KeyValueData>> GetAllSelectable();
        Task<List<KeyValueData>> GetAllSelectableBySellerId(long sellerId);
    }
}
