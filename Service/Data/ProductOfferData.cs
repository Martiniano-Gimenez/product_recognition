using Model.Utils;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class ProductOfferData
    {
        public long? Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Las unidades deben ser mayores a 0.")]
        public int Units { get; set; }

        [Range(typeof(decimal), "0,01", "9999999", ErrorMessage = "El precio unitario debe ser mayor a 0.")]
        public decimal UnitPrice { get; set; }

        public string UnitPriceDisplay => UnitPrice.AsMoneyString();
    }
}
