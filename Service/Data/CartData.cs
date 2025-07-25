using Model.Utils;

namespace Service.Data
{
    public class CartData
    {
        public string? Observation { get; set; }
        public long ClientId { get; set; }

        public List<CartDetailData> Products { get; set; } = new List<CartDetailData>();

        public decimal Net => Products.Sum(p => p.Net);
        public decimal Iva => Products.Sum(p => p.Iva);
        public decimal Total => Products.Sum(p => p.Total);
        public string NetDisplay => Net.AsMoneyString();
        public string IvaDisplay => Iva.AsMoneyString();
        public string TotalDisplay => Total.AsMoneyString();
    }
}
