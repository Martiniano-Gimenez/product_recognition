using Model.Domain;
using Model.Utils;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class ProductMappingExtensions
    {
        public static Expression<Func<Product, Product>> MapForGridDataForShoppingCart()
        {
            return entity => new Product
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                SalePrice = entity.SalePrice,
                IvaPercentage = entity.IvaPercentage
            };
        }

        public static GridData<ProductGridData> MapToPagGridDataForShoppingCart(this PagingResult<Product> entity)
        {
            return new GridData<ProductGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridDataForShoppingCart()).ToList()
            };
        }

        public static ProductGridData MapToGridDataForShoppingCart(this Product entity)
        {
            return new ProductGridData
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
            };
        }

        public static Expression<Func<Product, Product>> MapForGridData()
        {
            return entity => new Product
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                SalePrice = entity.SalePrice,
                IvaPercentage = entity.IvaPercentage
            };
        }

        public static GridData<ProductGridData> MapToPagGridData(this PagingResult<Product> entity)
        {
            return new GridData<ProductGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridData()).ToList()
            };
        }

        public static ProductGridData MapToGridData(this Product entity)
        {
            return new ProductGridData
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                IvaPercentage = entity.IvaPercentage.AsMoneyString(false),
                Price = entity.SalePrice.AsMoneyString(true),
            };
        }

        public static Expression<Func<Product, ProductData>> MapToData()
        {
            return entity => new ProductData
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                SalePrice = entity.SalePrice,
                IvaPercentage = entity.IvaPercentage,   
                Description = entity.Description
            };
        }

        public static Expression<Func<Product, KeyValueData>> MapForGetByTerm()
        {
            return entity => new KeyValueData
            {
                Key = entity.Id,
                Value = entity.CodeAndNameAndDescription
            };
        }

        public static Expression<Func<Product, OrderDetailData>> MapToOrderDetailData(int quantity)
        {
            return entity => new OrderDetailData
            {
                ProductId = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                UnitPrice = entity.SalePrice,
                Quantity = quantity,
                IvaPercentage = entity.IvaPercentage
            };
        }

        public static Expression<Func<Product, DepositMovementDetailData>> MapToDepositMovementDetailData(int quantity)
        {
            return entity => new DepositMovementDetailData
            {
                ProductId = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Quantity = quantity,
            };
        }

        public static Expression<Func<Product, Product>> MapWithSalePriceAndProductOffers()
        {
            return entity => new Product
            {
                Id = entity.Id,
                SalePrice = entity.SalePrice,
                IvaPercentage = entity.IvaPercentage,
            };
        }

        public static Product MapToEntity(this ProductData data)
        {
            return new Product
            {
                Code = data.Code,
                Name = data.Name,
                SalePrice = data.SalePrice,
                Description = data.Description,
                IvaPercentage = data.IvaPercentage,
            };
        }

        public static Product MapToEntity(this ProductData data, Product entity)
        {
            entity.Code = data.Code;
            entity.Name = data.Name;
            entity.SalePrice = data.SalePrice;
            entity.Description = data.Description;
            entity.IvaPercentage = data.IvaPercentage;

            return entity;
        }

        public static Expression<Func<Product, ProductViewData>> MapToViewData()
        {
            return entity => new ProductViewData
            {
                Code = entity.Code,
                Name = entity.Name,
                SalePrice = entity.SalePrice,
                IvaPercentage = entity.IvaPercentage,
                Description = entity.Description
            };
        }
    }
}
