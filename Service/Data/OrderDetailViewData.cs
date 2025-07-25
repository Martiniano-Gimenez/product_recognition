using Model.Utils;

namespace Service.Data
{
    public class OrderDetailViewData
    {
        public long ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total {  get; set; } 

        public string UnitPriceDisplay => UnitPrice.AsMoneyString();
        public string TotalDisplay => Total.AsMoneyString();
    }
}
