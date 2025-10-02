using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class DepositMovementDetailMappingExtensions
    {
        public static Expression<Func<DepositMovementDetail, DepositMovementDetailData>> MapToDepositMovementDetailData()
        {
            return entity => new DepositMovementDetailData
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                Code = entity.Product.Code,
                Name = entity.Product.Name,
                Quantity = entity.Quantity
            };
        }

        public static Expression<Func<DepositMovementDetail, DepositMovementDetailViewData>> MapToDepositMovementDetailViewData()
        {
            return entity => new DepositMovementDetailViewData
            {
                ProductId = entity.ProductId,   
                Code = entity.Product.Code,
                Name = entity.Product.Name,
                Quantity = entity.Quantity
            };
        }

        public static DepositMovementDetail MapToEntity(this DepositMovementDetailData data)
        {
            return new DepositMovementDetail
            {
                ProductId = data.ProductId,
                Quantity = data.Quantity
            };
        }

        public static DepositMovementDetail MapToEntity(this DepositMovementDetail entity, DepositMovementDetailData? data)
        {
            if (data is null)
                entity.IsActive = false;
            else
                entity.Quantity = data.Quantity;
            return entity;
        }
    }
}
