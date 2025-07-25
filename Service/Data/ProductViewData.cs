using Model.Utils;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class ProductViewData
    {
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Precio")]
        public decimal SalePrice { get; set; }

        [Display(Name = "% Iva")]
        public decimal IvaPercentage { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public List<ProductOfferViewData> ProductOffers { get; set; } = new List<ProductOfferViewData>();

        public string SalePriceDisplay => SalePrice.AsMoneyString();
    }
}
