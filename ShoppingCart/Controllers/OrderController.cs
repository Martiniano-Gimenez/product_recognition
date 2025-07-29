using Microsoft.AspNetCore.Mvc;
using Model.Domain;
using Service;
using Service.Data;
using Service.Implementations;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;
using ShoppingCart.Helpers;

namespace ShoppingCart.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IClientService _clientService;

        public OrderController(IOrderService orderService, 
                               IProductService productService,
                               IClientService clientService)
        {
            _orderService = orderService;
            _productService = productService;
            _clientService = clientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param)
        {
            GridData<OrderGridData> gridData = await _orderService.GetAllPaginated(param, User.GetUserId(), User.GetRoleId());
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                ViewBag.Clients = await _clientService.GetAllSelectable();
                return View(await _orderService.GetById(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderData data)
        {
            try
            {
                if (await _orderService.Edit(data, User.GetUserId()))
                    ShowSuccessMessage("El pedido fue actualizado con éxito");
                else
                    ShowErrorMessage("Ocurrio un error al actualizar el pedido, intentelo nuevamente");
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetProductsByTerm(string term)
        {
            var products = await _productService.GetAllSelectableByTerm(term);
            return Json(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToOrder(OrderData data)
        {
            ModelState.Clear();
            return PartialView("_edit", await _orderService.AddProductToOrder(data));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(OrderData data)
        {
            ModelState.Clear();
            return PartialView("_edit", data);
        }

        [HttpPost]
        public IActionResult DeleteProduct(OrderData data, long productId)
        {
            ModelState.Clear();
            data.Products = data.Products.Where(p => p.ProductId != productId).ToList();
            return PartialView("_edit", data);
        }

        public async Task<IActionResult> Detail(long id)
        {
            try
            {
                return View(await _orderService.GetByIdForView(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approve(long id)
        {
            return Json(await _orderService.Approve(id, User.GetUserId()));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(long id)
        {
            return Json(await _orderService.Reject(id, User.GetUserId()));
        }
    }
}
