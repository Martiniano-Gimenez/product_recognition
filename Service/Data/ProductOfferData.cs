using Model.Utils;

namespace Service.Data
{
    public class ProductOfferData
    {
        public long? Id { get; set; }
        public int Units { get; set; }
        public decimal UnitPrice { get; set; }

        public string UnitPriceDisplay => UnitPrice.AsMoneyString();
    }
}
