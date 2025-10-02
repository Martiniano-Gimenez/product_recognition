using Service.Data;

namespace Service.ServiceContracts
{
    public interface IDepositMovementService
    {
        Task<List<KeyValueData>> GetAllDepositsSelectable();
        Task<GridData<DepositMovementGridData>> GetAllPaginated(DTParameters param, long userId, short roleId);
        Task<DepositMovementData> GetById(long id);
        Task<DepositMovementViewData> GetByIdForView(long id);
        Task<DepositMovementData> AddProductToDepositMovement(DepositMovementData data);
        Task<bool> Create(DepositMovementData data, long userId);
        Task<bool> Edit(DepositMovementData data, long userId);
    }
}
