using Microsoft.AspNetCore.Http;
using Service.Resources;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class SalesFileData
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Archivo")]
        public IFormFile File { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Orden")]
        public int? Order { get; set; }

        public string? Name {  get; set; }
    }
}
