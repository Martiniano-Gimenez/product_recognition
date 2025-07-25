using Service.Resources;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class ProductData
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Precio")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "% Iva")]
        public decimal IvaPercentage { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Ofertas")]
        public List<ProductOfferData> ProductOffers { get; set; } = new List<ProductOfferData>();


        [Display(Name = "Unidades")]
        public int? AddOfferUnits { get; set; }

        [Display(Name = "Precio Unitario")]
        public decimal? AddOfferUnitPrice { get; set; }

        public void AddProductOffer()
        {
            if (!AddOfferUnits.HasValue || !AddOfferUnitPrice.HasValue || ProductOffers.Any(pd => pd.Units == AddOfferUnits))
                return;

            ProductOffers.Add(new ProductOfferData { Units = AddOfferUnits.Value, UnitPrice = AddOfferUnitPrice.Value });

            ProductOffers = ProductOffers.OrderByDescending(po => po.Units).ToList();
            return;
        }
    }
}
