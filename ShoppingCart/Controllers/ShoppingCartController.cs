using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Domain;
using Model.Utils;
using Service;
using Service.Data;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;
using ShoppingCart.Helpers;

namespace ShoppingCart.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;
        private readonly IGroupService _groupService;
        private readonly ICategoryService _categoryService;

        public ShoppingCartController(IProductService productService,
                                      ICartService cartService,
                                      IOrderService orderService,
                                      IClientService clientService,
                                      IGroupService groupService,
                                      ICategoryService categoryService)
        {
            _productService = productService;
            _cartService = cartService;
            _orderService = orderService;
            _clientService = clientService;
            _groupService = groupService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param)
        {
            GridData<ProductGridData> gridData = await _productService.GetAllPaginatedForShoppingCart(param);
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        [HttpPost]
        public async Task<IActionResult> GetProductDetail(long productId)
        {
            return PartialView("_productDetail", await _productService.GetShoppingCartProductDetailData(productId));
        }

        [HttpPost]
        public async Task<IActionResult> GetProductPriceByQuantity(long productId, int quantity)
        {
            try
            {
                var result = await _productService.GetPriceByQuantity(productId, quantity);
                return Json(new { price = result.AsMoneyString() });
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart(long productId, int quantity)
        {
            try
            {
                if (!await _cartService.AddProductToCart(User.GetUserId(), productId, quantity))
                    return StatusCode(500);
                return PartialView("_cart", await _cartService.GetUserCart(User.GetUserId()));
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        public async Task<IActionResult> Cart()
        {
            ViewBag.Clients = await _clientService.GetAllSelectable();
            return View(await _cartService.GetUserCart(User.GetUserId()));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProductCartQuantity(long productId, int quantity)
        {
            try
            {
                if (!await _cartService.ChangeProductCartQuantity(User.GetUserId(), productId, quantity))
                    return StatusCode(500);
                return PartialView("_cart", await _cartService.GetUserCart(User.GetUserId()));
            }
            catch(BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductFromCart(long productId)
        {
            var result = await _cartService.DeleteProductFromCart(User.GetUserId(), productId);
            if (!result)
                return StatusCode(500);
            return PartialView("_cart", await _cartService.GetUserCart(User.GetUserId()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CartData data)
        {
            try
            {
                if (await _orderService.Create(data, User.GetUserId()))
                    ShowSuccessMessage("Su pedido fue generado con éxito");
                else
                    ShowErrorMessage("Ocurrio un error al generar su pedido, intentelo nuevamente");
                return User.GetRoleId() == (short)eRole.Purchasing ? RedirectToAction("Orders") : RedirectToAction("Index", "Order");
            }
            catch(BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Cart");
            }
        }

        public async Task<IActionResult> Orders()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OrdersGrid([FromBody] DTParameters param)
        {
            GridData<OrderGridData> gridData = await _orderService.GetAllPaginatedForShoppingCart(param, User.GetUserId());
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        public async Task<IActionResult> OrderDetail(long id)
        {
            try
            {
                return View(await _orderService.GetByIdForView(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Orders");
            }
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetProductsByTerm(string term)
        {
            var products = await _productService.GetAllSelectableByTerm(term);
            return Json(products);
        }
    }
}
