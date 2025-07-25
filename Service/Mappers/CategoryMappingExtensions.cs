using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class CategoryMappingExtensions
    {
        public static Expression<Func<Category, KeyValueData>> MapToKeyValueData()
        {
            return entity => new KeyValueData
            {
                Key = entity.Id,
                Value = entity.CodeAndName,
            };
        }
    }
}
