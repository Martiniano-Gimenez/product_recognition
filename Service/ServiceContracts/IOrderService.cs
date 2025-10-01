using Service.Data;

namespace Service.ServiceContracts
{
    public interface IOrderService
    {
        Task<GridData<OrderGridData>> GetAllPaginatedForShoppingCart(DTParameters param, long userId);
        Task<GridData<OrderGridData>> GetAllPaginated(DTParameters param, long userId, short roleId);
        Task<bool> Create(CartData data, long userId);
        Task<OrderData> GetById(long id);
        Task<OrderViewData> GetByIdForView(long id);
        Task<OrderData> AddProductToOrder(OrderData data);
        Task<bool> Create(OrderData data, long userId);
        Task<bool> Edit(OrderData data, long userId);
        Task<bool> Approve(long orderId, long userId);
        Task<bool> Reject(long orderId, long userId);
    }
}
