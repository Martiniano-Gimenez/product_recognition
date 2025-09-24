using Microsoft.AspNetCore.Http;
using Service.Resources;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class ProductImageData
    {
        public long ProductId { get; set; }
        public string? Path { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Archivo")]
        public IFormFile File { get; set; }
    }
}
