using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class OrderDetailMappingExtensions
    {
        public static Expression<Func<OrderDetail, OrderDetailData>> MapToOrderDetailData()
        {
            return entity => new OrderDetailData
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                Code = entity.Product.Code,
                Name = entity.Product.Name,
                UnitPrice = entity.UnitPrice,
                Quantity = entity.Quantity,
                IvaPercentage = entity.IvaPercentage,
            };
        }

        public static Expression<Func<OrderDetail, OrderDetailViewData>> MapToOrderDetailViewData()
        {
            return entity => new OrderDetailViewData
            {
                ProductId = entity.ProductId,   
                Code = entity.Product.Code,
                Name = entity.Product.Name,
                UnitPrice = entity.UnitPrice,
                Quantity = entity.Quantity,
                Total = entity.Total
            };
        }

        public static OrderDetail MapToEntity(this OrderDetailData data)
        {
            return new OrderDetail
            {
                ProductId = data.ProductId,
                Quantity = data.Quantity,
                UnitPrice = data.UnitPrice,
                Total = data.Total,
                IvaPercentage = data.IvaPercentage
            };
        }

        public static OrderDetail MapToEntity(this OrderDetail entity, OrderDetailData? data)
        {
            if (data is null)
                entity.IsActive = false;
            else
            {
                entity.Quantity = data.Quantity;
                entity.UnitPrice = entity.Product.SalePrice;
                entity.Total = data.Quantity * entity.UnitPrice;
                entity.IvaPercentage = data.IvaPercentage;
            }
            return entity;
        }
    }
}
