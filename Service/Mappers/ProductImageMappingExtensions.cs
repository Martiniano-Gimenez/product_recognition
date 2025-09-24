using Model.Domain;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class ProductImageMappingExtensions
    {
        public static Expression<Func<ProductImage, ProductImage>> MapForGridData()
        {
            return entity => new ProductImage
            {
                Id = entity.Id,
                Path = entity.Path,
            };
        }

        public static GridData<ProductImageGridData> MapToPagGridData(this PagingResult<ProductImage> entity)
        {
            return new GridData<ProductImageGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridData()).ToList()
            };
        }

        public static ProductImageGridData MapToGridData(this ProductImage entity)
        {
            return new ProductImageGridData
            {
                Id = entity.Id,
                Path = entity.Path
            };
        }

        public static ProductImage MapToEntity(this ProductImageData data)
        {
            return new ProductImage
            {
                ProductId = data.ProductId,
                Path = data.Path,
            };
        }
    }
}
