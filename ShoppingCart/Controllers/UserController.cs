using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Data;
using Service.Implementations;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;

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

        public IActionResult Create()
        {
            ViewBag.Roles = _userService.GetAllRoles();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserData data)
        {
            try
            {
                ModelState.Remove("UserId");
                if (ModelState.IsValid)
                {
                    if (await _userService.Create(data))
                        ShowSuccessMessage("El usuario fue creado con éxito");
                    else
                        ShowErrorMessage("Ocurrio un error al crear el usuario, intentelo nuevamente");
                    return RedirectToAction("Index");
                }
                ViewBag.Roles = _userService.GetAllRoles();
                return View(data);
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                ViewBag.Roles = _userService.GetAllRoles();
                return View(await _userService.GetById(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserData data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _userService.Edit(data))
                        ShowSuccessMessage("El usuario fue actualizado con éxito");
                    else
                        ShowErrorMessage("Ocurrio un error al actualizar el usuario, intentelo nuevamente");
                    return RedirectToAction("Index");
                }
                ViewBag.Roles = _userService.GetAllRoles();
                return View(data);
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            try
            {
                var result = await _userService.Delete(id);
                return Json(new { result });
            }
            catch (BusinessException ex)
            {
                return Json(new { result = false, msg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(long userId)
        {
            return Json(await _userService.ResetPassword(userId));
        }
    }
}
