using Service.Resources;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class UserData
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Rol")]
        public int RoleId { get; set; }
        public string? DisplayName { get; set; }
        public long UserId { get; set; }
        public bool HasToChangePassword { get; set; }
    }
}
