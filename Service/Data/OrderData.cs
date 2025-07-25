using Model.Utils;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class OrderData
    {
        public long Id { get; set; }

        [Display(Name = "Cliente")]
        public string Client { get; set; }

        [Display(Name = "Vendedor")]
        public string Seller { get; set; }

        [Display(Name = "Fecha")]
        public string Date { get; set; }

        [Display(Name = "Observación")]
        public string? Observation { get; set; }

        [Display(Name = "Agregar artículo")]
        public long? NewProductId { get; set; }

        [Display(Name = "Cantidad")]
        public int? NewProductQuantity { get; set; }

        public List<OrderDetailData> Products { get; set; } = new List<OrderDetailData>();

        public decimal Net => Products.Sum(p => p.Net);
        public decimal Iva => Products.Sum(p => p.Iva);
        public decimal Total => Products.Sum(p => p.Total);
        public string NetDisplay => Net.AsMoneyString();
        public string IvaDisplay => Iva.AsMoneyString();
        public string TotalDisplay => Total.AsMoneyString();
    }
}
