using Model.Attributes;
using Model.Domain;
using Model.Utils;
using System.ComponentModel;

namespace Service.Data
{
    public class ProductGridData : BaseGridData
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string IvaPercentage { get; set; }
        public int Units { get; set; } = 1;
        public int OffersQuantity { get; set; }
        public string? OffersAsStringForTooltip { get; set; }

        public bool ShowOffersAsStringForTooltip => !string.IsNullOrWhiteSpace(OffersAsStringForTooltip);
    }
}
