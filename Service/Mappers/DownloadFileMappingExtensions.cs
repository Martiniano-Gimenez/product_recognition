using Model.Domain;
using Model.Utils;
using Service.Data;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class DownloadFileMappingExtensions
    {
        public static Expression<Func<DownloadFile, DownloadFile>> MapForGridData()
        {
            return entity => new DownloadFile
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order,
            };
        }

        public static GridData<DownloadFileGridData> MapToPagGridData(this PagingResult<DownloadFile> entity)
        {
            return new GridData<DownloadFileGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridData()).ToList()
            };
        }

        public static DownloadFileGridData MapToGridData(this DownloadFile entity)
        {
            return new DownloadFileGridData
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order.ToString()
            };
        }

        public static DownloadFile MapToEntity(this DownloadFileData data)
        {
            return new DownloadFile
            {
                Name = data.Name,
                Order = data.Order.Value,
                FileExtension = data.FileExtension,
            };
        }

        public static Expression<Func<DownloadFile, DownloadFile>> MapForDisplay()
        {
            return entity => new DownloadFile
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order,
                FileExtension = entity.FileExtension,
            };
        }
    }
}
