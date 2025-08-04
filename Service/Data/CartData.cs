using Model.Utils;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class CartData
    {
        public string? Observation { get; set; }

        [Display(Name = "Cliente")]
        public long ClientId { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Agregar artículo")]
        public long? NewProductId { get; set; }

        [Display(Name = "Cantidad")]
        public int? NewProductQuantity { get; set; }

        public List<CartDetailData> Products { get; set; } = new List<CartDetailData>();

        public decimal Net => Products.Sum(p => p.Net);
        public decimal Iva => Products.Sum(p => p.Iva);
        public decimal Total => Products.Sum(p => p.Total);
        public string NetDisplay => Net.AsMoneyString();
        public string IvaDisplay => Iva.AsMoneyString();
        public string TotalDisplay => Total.AsMoneyString();
        public string DateDisplay => Date.ToString("dd/MM/yyyy");
    }
}
