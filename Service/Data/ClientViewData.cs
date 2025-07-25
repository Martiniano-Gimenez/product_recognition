using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class ClientViewData
    {
        [Display(Name = "Razón social")]
        public string? Name { get; set; }

        [Display(Name = "CUIT/CUIL")]
        public string? CUIL { get; set; }

        [Display(Name = "Vendedor")]
        public string? Seller { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Celular")]
        public long? CellPhone { get; set; }

        [Display(Name = "Teléfono")]
        public long? Telephone { get; set; }
    }
}
