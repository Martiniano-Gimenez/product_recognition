using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class GroupMappingExtensions
    {
        public static Expression<Func<Group, KeyValueData>> MapToKeyValueData()
        {
            return entity => new KeyValueData
            {
                Key = entity.Id,
                Value = entity.CodeAndName,
            };
        }
    }
}
