using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddProductToCart(long userId, long productId, int quantity)
        {
            if (userId <= 0 || productId <= 0 || quantity <= 0)
                return false;

            long cartId = await _unitOfWork.CartRepository.GetByUserId(userId).Select(cc => cc.Id).FirstOrDefaultAsync();

            if (cartId <= 0)
                await _unitOfWork.CartRepository.CreateAsync(CartMappingExtensions.MapToEntityForAddProductToCart(userId, productId, quantity));
            else
            {
                var cartDetail = await _unitOfWork.CartDetailRepository.GetByUserIdAndProductId(userId, productId).FirstOrDefaultAsync();

                if (cartDetail is null)
                    await _unitOfWork.CartDetailRepository.CreateAsync(CartDetailMappingExtensions.MapToEntityForAddProductToCart(productId, cartId, quantity));
                else
                    cartDetail.Quantity += quantity;
            }

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<CartData> GetUserCart(long userId)
        {
            return await _unitOfWork.CartRepository.GetByUserId(userId)
                .Select(CartMappingExtensions.MapToData()).FirstOrDefaultAsync() ?? new CartData();
        }

        public async Task<bool> ChangeProductCartQuantity(long userId, long productId, int quantity)
        {
            var cartDetail = await _unitOfWork.CartDetailRepository.GetByUserIdAndProductId(userId, productId).FirstOrDefaultAsync();
            if (cartDetail == null)
                return false;

            cartDetail.Quantity = quantity;
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductFromCart(long userId, long productId)
        {
            var cartDetail = await _unitOfWork.CartDetailRepository.GetByUserIdAndProductId(userId, productId).FirstOrDefaultAsync();
            if (cartDetail == null)
                return false;

            _unitOfWork.CartDetailRepository.Remove(cartDetail);
            return await _unitOfWork.SaveChangesAsync();    
        }
    }
}
