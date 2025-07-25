using Model.Attributes;
using Model.Domain;
using Model.Utils;
using System.ComponentModel;

namespace Service.Data
{
    public class CartDetailData
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get;set; }
        public string Product { get; set; }
        public decimal IvaPercentage {  get; set; }
        public eProductAvailabilityState ProductAvailabilityStateId { get; set; }

        public string DisplayPrice => Price.AsMoneyString();
        public decimal Net => Price * Quantity;
        public string DisplayNet => Net.AsMoneyString();
        public decimal Iva => Price * (IvaPercentage / 100) * Quantity;
        public decimal Total => Net + Iva;
        public string AvailabilityStateDescription => ProductAvailabilityStateId.GetAttribute<DescriptionAttribute>().Description;
        public string AvailabilityStateClass => ProductAvailabilityStateId.GetAttribute<ClassesAtributte>().Classes;
    }
}
