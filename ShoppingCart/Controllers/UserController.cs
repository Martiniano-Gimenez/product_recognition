using Microsoft.AspNetCore.Mvc;
using Service.Data;
using Service.Implementations;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;
using ShoppingCart.Helpers;

namespace ShoppingCart.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param)
        {
            GridData<UserGridData> gridData = await _userService.GetAllPaginated(param);
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(long userId)
        {
            return Json(await _userService.ResetPassword(userId));
        }
    }
}
