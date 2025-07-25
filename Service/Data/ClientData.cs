using Service.Resources;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class ClientData
    {
        public long? Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Razón social")]
        public string? Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "CUIT/CUIL")]
        public string? CUIL { get; set; }

        [Display(Name = "Vendedor")]
        public long? SellerId { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Celular")]
        public long? CellPhone { get; set; }

        [Display(Name = "Teléfono")]
        public long? Telephone { get; set; }
    }
}
