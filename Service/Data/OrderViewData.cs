using Model.Attributes;
using Model.Domain;
using Model.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class OrderViewData
    {
        public long Id { get; set; }

        [Display(Name = "Cliente")]
        public string Client { get; set; }

        [Display(Name = "Vendedor")]
        public string Seller { get; set; }

        [Display(Name = "Fecha")]
        public string Date { get; set; }

        [Display(Name = "Estado actual")]
        public eOrderState OrderState { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Observación")]
        public string? Observation { get; set; }


        public List<OrderDetailViewData> Products { get; set; } = new List<OrderDetailViewData>();

        public decimal Net => Products.Sum(p => p.Net);
        public decimal Iva => Products.Sum(p => p.Iva);
        public string NetDisplay => Net.AsMoneyString();
        public string IvaDisplay => Iva.AsMoneyString();
        public string TotalDisplay => Total.AsMoneyString();
        public string StateDescription => OrderState.GetAttribute<DescriptionAttribute>().Description;
        public string StateClass => OrderState.GetAttribute<ClassesAtributte>().Classes;
    }
}
