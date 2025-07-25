using Service.Resources;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class LoginData
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}
