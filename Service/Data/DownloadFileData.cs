using Microsoft.AspNetCore.Http;
using Model.Domain;
using Service.Resources;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class DownloadFileData
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Archivo")]
        public IFormFile File { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Orden")]
        public int? Order { get; set; }

        public string? Name { get; set; }
        public eFileExtension FileExtension { get; set; }
    }
}
