using Service.Data;

namespace Service.ServiceContracts
{
    public interface ICartService
    {
        Task<bool> AddProductToCart(long userId, long productId, int quantity);
        Task<CartData> GetUserCart(long userId);
        Task<bool> ChangeProductCartQuantity(long userId, long productId, int quantity);
        Task<bool> DeleteProductFromCart(long userId, long productId);
    }
}
