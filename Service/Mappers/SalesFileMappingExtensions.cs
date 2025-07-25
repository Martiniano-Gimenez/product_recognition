using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class SalesFileMappingExtensions
    {
        public static Expression<Func<SalesFile, SalesFile>> MapForGridData()
        {
            return entity => new SalesFile
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order
            };
        }

        public static GridData<SalesFileGridData> MapToPagGridData(this PagingResult<SalesFile> entity)
        {
            return new GridData<SalesFileGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridData()).ToList()
            };
        }

        public static SalesFileGridData MapToGridData(this SalesFile entity)
        {
            return new SalesFileGridData
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order.ToString()
            };
        }

        public static SalesFile MapToEntity(this SalesFileData data)
        {
            return new SalesFile
            {
                Name = data.Name,
                Order = data.Order.Value,
            };
        }
    }
}
