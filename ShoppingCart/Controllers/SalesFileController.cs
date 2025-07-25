using Microsoft.AspNetCore.Mvc;
using Service.Data;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;

namespace ShoppingCart.Controllers
{
    public class SalesFileController : BaseController
    {
        private readonly ISalesFileService _salesFileService;
        private readonly string _directoryFiles = Path.Combine(Directory.GetCurrentDirectory(), "Files", "SalesFiles", "PDF");

        public SalesFileController(ISalesFileService salesFileService)
        {
            _salesFileService = salesFileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param)
        {
            GridData<SalesFileGridData> gridData = await _salesFileService.GetAllPaginated(param);
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SalesFileData data)
        {
            if (ModelState.IsValid && data.File != null && data.File.Length > 0)
            {
                using var stream = data.File.OpenReadStream();
                data.Name = data.File.FileName;
                await _salesFileService.Create(stream, data);
                return RedirectToAction("Index");
            }
            return View("Index", data);
        }

        public async Task<JsonResult> UpdateOrder(long id, int order)
        {
            return Json(await _salesFileService.UpdateOrder(id, order));
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

        [HttpGet("/api/pdf/{id}")]
        public async Task<IActionResult> GetPdf(long id)
        {
            var filePath = Path.Combine(_directoryFiles, $"{id}.pdf");

            if (!System.IO.File.Exists(filePath))
                return NotFound("Archivo no encontrado.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            string name = await _salesFileService.GetFileName(id);
            return File(fileBytes, "application/pdf", name);
        }
    }
}
