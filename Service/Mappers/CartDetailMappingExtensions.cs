using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class CartDetailMappingExtensions
    {
        public static CartDetail MapToEntityForAddProductToCart(long productId, long clientCartId, int quantity)
        {
            return new CartDetail
            {
                CartId = clientCartId,
                ProductId = productId,
                Quantity = quantity
            };
        }
    }
}
