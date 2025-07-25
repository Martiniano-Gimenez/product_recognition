using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Controllers.Base
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "Role")]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class BaseController : Controller
    {
        private readonly string SUCCESS_MESSAGE_KEY = "successMessage";
        private readonly string ERROR_MESSAGE_KEY = "errorMessage";

        public void ShowSuccessMessage(string message)
        {
            TempData.Clear();
            if (!string.IsNullOrWhiteSpace(message))
                TempData.Add(SUCCESS_MESSAGE_KEY, message);
        }

        public void ShowErrorMessage(string message)
        {
            TempData.Clear();
            if (!string.IsNullOrWhiteSpace(message))
                TempData.Add(ERROR_MESSAGE_KEY, message);
        }
    }
}
