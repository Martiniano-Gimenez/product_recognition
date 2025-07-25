using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class OrderHistoryMappingExtensions
    {
        public static Expression<Func<OrderHistory, OrderHistoryViewData>> MapToViewData()
        {
            return entity => new OrderHistoryViewData
            {
                Date = entity.Date.ToString("dd/MM/yyyy HH:mm"),
                OrderState = entity.OrderState,
            };
        }
    }
}
