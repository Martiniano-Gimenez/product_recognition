using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class SellerMappingExtensions
    {
        public static Expression<Func<Seller, KeyValueData>> MapToKeyValueData()
        {
            return entity => new KeyValueData
            {
                Key = entity.Id,
                Value = entity.Name
            };
        }
    }
}
