using Microsoft.AspNetCore.Mvc;
using Service.Data;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;

namespace ShoppingCart.Controllers
{
    public class ProductImageController : BaseController
    {
        private readonly IProductImageService _salesFileService;
        private readonly string _directoryFiles = Path.Combine(Directory.GetCurrentDirectory(), "Files", "SalesFiles", "PDF");

        public ProductImageController(IProductImageService salesFileService)
        {
            _salesFileService = salesFileService;
        }

        public IActionResult Index(long productId)
        {
            return View(new ProductImageData { ProductId = productId });
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param, long productId)
        {
            GridData<ProductImageGridData> gridData = await _salesFileService.GetAllPaginated(productId);
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductImageData data)
        {
            if (ModelState.IsValid && data.File != null && data.File.Length > 0)
            {
                using var stream = data.File.OpenReadStream();
                await _salesFileService.Create(stream, data, data.File.FileName);
                return RedirectToAction("Index", new { productId = data.ProductId });
            }
            return View("Index", data);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            return Json(await _salesFileService.Delete(id));
        }

        public async Task<IActionResult> Display()
        {
            ViewBag.Offset = 8;
            return View(await _salesFileService.GetForDisplay());
        }

        [HttpPost]
        public async Task<IActionResult> LoadPrevious(int offset)
        {
            ViewBag.Offset = offset + 4;
            return PartialView("_display", await _salesFileService.GetForDisplay(offset, 4));
        }
    }
}
