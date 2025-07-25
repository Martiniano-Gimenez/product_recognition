using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Controllers
{
    public class ErrorController : Controller
    {
        public async Task<IActionResult> Error()
        {
            return View();
        }

        public async Task<IActionResult> UnAuthorized()
        {
            return View();
        }

        public async Task<IActionResult> NotFound()
        {
            return View();
        }
    }
}
