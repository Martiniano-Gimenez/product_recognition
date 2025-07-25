using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class CartMappingExtensions
    {
        public static Cart MapToEntityForAddProductToCart(long userId, long productId, int quantity)
        {
            return new Cart
            {
                UserId = userId,
                CartDetails = new List<CartDetail> { new CartDetail { ProductId = productId, Quantity = quantity }}
            };
        }

        public static Expression<Func<Cart, CartData>> MapToData()
        {
            return entity => new CartData
            {
                Products = entity.CartDetails.Select(cd => new CartDetailData
                {
                    ProductId = cd.ProductId,
                    Price = cd.Product.SalePrice,
                    Quantity = cd.Quantity,
                    Product = cd.Product.Name,
                    IvaPercentage = cd.Product.IvaPercentage,
                }).ToList()
            };
        }

        public static Order MapToOrder(this CartData data, long clientId, long? sellerId)
        {
            return new Order
            {
                ClientId = clientId,
                SellerId = sellerId,
                Total = data.Total,
                Observation = data.Observation,
                OrderDetails = data.Products.Select(p => new OrderDetail
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    UnitPrice = p.Price,
                    Net = p.Net,
                    IvaPercentage = p.IvaPercentage,
                    Total = p.Total
                }).ToList(),
            };
        }
    }
}
