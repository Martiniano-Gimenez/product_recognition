using Model.Utils;

namespace Service.Data
{
    public class OrderDetailData
    {
        public long? Id { get; set; }
        public long ProductId { get; set; }
        public string Code { get; set; } 
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal IvaPercentage { get; set; }

        
        public decimal Net => UnitPrice * Quantity;
        public decimal Iva => UnitPrice * (IvaPercentage / 100) * Quantity;
        public decimal Total => Net + Iva;
        public string DisplayTotal => Total.AsMoneyString();
    }
}
