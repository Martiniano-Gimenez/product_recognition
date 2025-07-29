using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Data;
using ShoppingCart.Controllers.Base;
using ShoppingCart.Helpers;
using System.Security.Claims;
using Service.ServiceContracts;
using Service;

namespace ShoppingCart.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginData data)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = await _userService.Login(data);
                if (loggedUser != null)
                {
                    CreateLoginClaims(loggedUser);
                    await SignIn(User.Claims, DateTimeOffset.UtcNow.AddHours(12));

                    if (loggedUser.HasToChangePassword)
                    {
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return RedirectToAction("ChangePassword", new { userId = loggedUser.UserId });
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                    ShowErrorMessage("Usuario/Contraseña incorrecto");
            }

            return View(data);
        }

        public async Task<IActionResult> ChangePassword(long userId)
        {
            return View(new ChangePasswordData { Id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordData data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _userService.ChangePassword(data))
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
            } 
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            return View(data);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private void CreateLoginClaims(UserData data)
        {
            User.AddOrUpdateClaim(ClaimTypes.Name, data.UserName);
            User.AddOrUpdateClaim("UserId", data.UserId.ToString());
            User.AddOrUpdateClaim("Role", data.RoleId.ToString());
            User.AddOrUpdateClaim("SellerId", data.SellerId?.ToString() ?? "0");
        }

        private async Task SignIn(IEnumerable<Claim> claims, DateTimeOffset expiresUtc)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = expiresUtc });
        }
    }
}
