using Model.Attributes;
using Model.Domain;
using Model.Utils;
using System.ComponentModel;

namespace Service.Data
{
    public class ShoppingCartProductDetailData
    {
        public eProductAvailabilityState ProductAvailabilityStateId { get; set; }
        public string? MinimumSalesQuantity { get; set; }
        public string? Group { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();

        public string AvailabilityStateDescription => ProductAvailabilityStateId.GetAttribute<DescriptionAttribute>().Description;
        public string AvailabilityStateClass => ProductAvailabilityStateId.GetAttribute<ClassesAtributte>().Classes;
    }
}
