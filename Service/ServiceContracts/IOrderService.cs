using Service.Data;

namespace Service.ServiceContracts
{
    public interface IOrderService
    {
        Task<GridData<OrderGridData>> GetAllPaginatedForShoppingCart(DTParameters param, long userId);
        Task<GridData<OrderGridData>> GetAllPaginated(DTParameters param, long userId, short roleId);
        Task<OrderData> GetById(long id);
        Task<OrderViewData> GetByIdForView(long id);
        Task<OrderData> AddProductToOrder(OrderData data);
        Task<OrderData> AddProductsToOrder(OrderData data, List<long> productIds);
        Task<bool> Create(OrderData data, long userId);
        Task<bool> Edit(OrderData data, long userId);
    }
}
