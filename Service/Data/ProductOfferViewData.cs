using Model.Utils;

namespace Service.Data
{
    public class ProductOfferViewData
    {
        public int Units { get; set; }
        public decimal UnitPrice { get; set; }

        public string DisplayUnitPrice => UnitPrice.AsMoneyString();
    }
}
