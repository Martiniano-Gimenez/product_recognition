using Microsoft.AspNetCore.Mvc;
using Service.Data;
using Service.Implementations;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;

namespace ShoppingCart.Controllers
{
    public class DownloadFileController : BaseController
    {
        private readonly IDownloadFileService _downloadFileService;
        private readonly string _pdfFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files", "DownloadFiles", "PDF");
        private readonly string _xlsxFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files", "DownloadFiles", "XLSX");

        public DownloadFileController(IDownloadFileService downloadFileService)
        {
            _downloadFileService = downloadFileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param, short fileExtensionId)
        {
            GridData<DownloadFileGridData> gridData = await _downloadFileService.GetAllPaginated(param, fileExtensionId);
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        [HttpPost]
        public async Task<IActionResult> Create(DownloadFileData data)
        {
            if (ModelState.IsValid && data.File != null && data.File.Length > 0)
            {
                using var stream = data.File.OpenReadStream();
                data.Name = data.File.FileName;
                await _downloadFileService.Create(stream, data);
                return RedirectToAction("Index");
            }
            return View("Index", data);
        }

        public async Task<JsonResult> UpdateOrder(long id, int order)
        {
            return Json(await _downloadFileService.UpdateOrder(id, order));
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            return Json(await _downloadFileService.Delete(id));
        }

        public async Task<IActionResult> Display()
        {
            ViewBag.OffsetPdf = 6;
            ViewBag.OffsetXlsx = 6;
            return View(await _downloadFileService.GetForDisplay());
        }

        [HttpPost]
        public async Task<IActionResult> LoadPrevious(int? offsetPdf, int? offsetXlsx)
        {
            ViewBag.OffsetPdf = offsetPdf.HasValue ? offsetPdf + 2 : offsetPdf;
            ViewBag.OffsetXlsx = offsetXlsx.HasValue ? offsetXlsx + 2 : offsetXlsx;
            return offsetPdf.HasValue 
                ? PartialView("_pdfContainer", await _downloadFileService.GetForDisplay(offsetPdf.Value, 2, 0, 0))
                : PartialView("_xlsxContainer", await _downloadFileService.GetForDisplay(0, 0, offsetXlsx ?? 0, 2));
        }

        [HttpGet("/api/file/{id}")]
        public async Task<IActionResult> GetFile(long id)
        {
            var fileData = await _downloadFileService.GetFileNameAndExtension(id);
            bool isPdf = fileData.FileExtension == Model.Domain.eFileExtension.Pdf;
            var filePath = Path.Combine(isPdf ? _pdfFilesDirectory : _xlsxFilesDirectory, $"{id}.{(isPdf ? "pdf" : "xlsx")}");

            if (!System.IO.File.Exists(filePath))
                return NotFound("Archivo no encontrado.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return isPdf
                ? File(fileBytes, "application/pdf", fileData.Name)
                : File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileData.Name); ;
        }
    }
}
