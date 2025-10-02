using Model.Domain;
using Model.Utils;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class OrderMappingExtensions
    {
        public static Expression<Func<Order, Order>> MapForGridData()
        {
            return entity => new Order
            {
                Id = entity.Id,
                Date = entity.Date,
                Total = entity.Total,
                Client = new Client { Name = entity.Client.Name }
            };
        }

        public static GridData<OrderGridData> MapToPagGridData(this PagingResult<Order> entity)
        {
            return new GridData<OrderGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridData()).ToList()
            };
        }

        public static OrderGridData MapToGridData(this Order entity)
        {
            return new OrderGridData
            {
                Id = entity.Id,
                Date = entity.Date.ToString("dd/MM/yyyy HH:mm"),
                Total = entity.Total.AsMoneyString(),
                Client = entity.Client.Name
            };
        }

        public static Expression<Func<Order, OrderData>> MapToOrderData()
        {
            return entity => new OrderData
            {
                Id = entity.Id,
                ClientId = entity.ClientId,
                Date = entity.Date.ToString("dd/MM/yyyy HH:mm"),
                Observation = entity.Observation,
                Products = entity.OrderDetails.AsQueryable().Where(od => od.IsActive)
                    .Select(OrderDetailMappingExtensions.MapToOrderDetailData()).ToList(),
            };
        }

        public static Expression<Func<Order, OrderViewData>> MapToOrderViewData()
        {
            return entity => new OrderViewData
            {
                Id = entity.Id,
                Client = entity.Client.Name,
                Date = entity.Date.ToString("dd/MM/yyyy HH:mm"),
                Total = entity.Total,
                Observation = entity.Observation,
                Products = entity.OrderDetails.AsQueryable().Where(od => od.IsActive)
                    .Select(OrderDetailMappingExtensions.MapToOrderDetailViewData()).ToList(),
            };
        }

        public static Order MapToEntity(this OrderData data)
        {
            return new Order
            {
                ClientId = data.ClientId.Value,
                Total = data.Total,
                Observation = data.Observation,
                OrderDetails = data.Products.Select(p => new OrderDetail
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice,
                    Net = p.Net,
                    IvaPercentage = p.IvaPercentage,
                    Total = p.Total
                }).ToList(),
            };
        }
    }
}
