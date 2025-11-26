using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Data;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;

namespace ShoppingCart.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param)
        {
            GridData<ProductGridData> gridData = await _productService.GetAllPaginated(param);
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        public IActionResult Create()
        {
            return View(new ProductData());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductData data)
        {
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    if (await _productService.Create(data))
                        ShowSuccessMessage("El artículo fue creado con éxito");
                    else
                        ShowErrorMessage("Ocurrio un error al crear el artículo, intentelo nuevamente");
                    return RedirectToAction("Index");
                }
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
                return View(await _productService.GetById(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductData data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _productService.Edit(data))
                        ShowSuccessMessage("El artículo fue actualizado con éxito");
                    else
                        ShowErrorMessage("Ocurrio un error al actualizar el artículo, intentelo nuevamente");
                    return RedirectToAction("Index");
                }
                return View(data);
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        public IActionResult AddOffer(ProductData data)
        {
            ModelState.Clear();
            data.AddProductOffer();
            return PartialView("_Offers", data);
        }

        public IActionResult DeleteOffer(ProductData data, int removeIndex)
        {
            ModelState.Clear();
            data.ProductOffers.RemoveAt(removeIndex);
            data.ProductOffers = data.ProductOffers.OrderByDescending(po => po.Units).ToList();
            return PartialView("_Offers", data);
        }

        public async Task<IActionResult> Detail(long id)
        {
            try
            {
                return View(await _productService.GetByIdForView(id));
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
            return Json(await _productService.Delete(id));
        }
    }
}
