using Model.Domain;
using Service.Data;

namespace Service.Mappers
{
    public static class ProductOfferMappingExtensions
    {
        public static ProductOffer MapToEntity(this ProductOfferData data)
        {
            return new ProductOffer
            {
                Units = data.Units,
                UnitPrice = data.UnitPrice,
            };
        }
    }
}
