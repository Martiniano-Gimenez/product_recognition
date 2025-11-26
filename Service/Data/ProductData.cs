using Service.Resources;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class ProductData
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El código debe ser alfanumérico.")]
        [StringLength(50, ErrorMessage = "El código no puede superar los 50 caracteres.")]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(120, ErrorMessage = "El nombre no puede superar los 120 caracteres.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Range(typeof(decimal), "0,01", "9999999", ErrorMessage = "El precio debe ser mayor a 0.")]
        [Display(Name = "Precio")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [RegularExpression(@"^(0(,0*)?|10,5(0*)|21(,0*)?|27(,0*)?)?$", ErrorMessage = "El IVA debe ser 0%, 10,5%, 21% o 27%.")]
        [Display(Name = "% Iva")]
        public decimal IvaPercentage { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Ofertas")]
        public List<ProductOfferData> ProductOffers { get; set; } = new List<ProductOfferData>();

        [Range(1, int.MaxValue, ErrorMessage = "Las unidades deben ser mayores a 0.")]
        [Display(Name = "Unidades")]
        public int? AddOfferUnits { get; set; }

        [Range(typeof(decimal), "0,01", "9999999", ErrorMessage = "El precio unitario debe ser mayor a 0.")]
        [Display(Name = "Precio Unitario")]
        public decimal? AddOfferUnitPrice { get; set; }

        public void AddProductOffer()
        {
            if (!AddOfferUnits.HasValue || !AddOfferUnitPrice.HasValue)
                return;

            if (ProductOffers.Any(p => p.Units == AddOfferUnits.Value))
                return;

            ProductOffers.Add(new ProductOfferData
            {
                Units = AddOfferUnits.Value,
                UnitPrice = AddOfferUnitPrice.Value
            });

            ProductOffers = ProductOffers.OrderByDescending(o => o.Units).ToList();
        }
    }
}
