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
    public class DepositMovementController : BaseController
    {
        private readonly IDepositMovementService _depositmovementService;
        private readonly IProductService _productService;
        private readonly IClientService _clientService;
        private readonly IProductDetectorService _productDetectorService;

        public DepositMovementController(IDepositMovementService depositmovementService, 
                               IProductService productService,
                               IClientService clientService,
                               IProductDetectorService productDetectorService)
        {
            _depositmovementService = depositmovementService;
            _productService = productService;
            _clientService = clientService;
            _productDetectorService = productDetectorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param)
        {
            GridData<DepositMovementGridData> gridData = await _depositmovementService.GetAllPaginated(param, User.GetUserId(), User.GetRoleId());
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Deposits = await _depositmovementService.GetAllDepositsSelectable();
                return View(new DepositMovementData());
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepositMovementData data)
        {
            try
            {
                if (await _depositmovementService.Create(data, User.GetUserId()))
                    ShowSuccessMessage("El movimiento fue creado con éxito");
                else
                    ShowErrorMessage("Ocurrio un error al crear el movimiento, intentelo nuevamente");
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                ViewBag.Deposits = await _depositmovementService.GetAllDepositsSelectable();
                return View(await _depositmovementService.GetById(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepositMovementData data)
        {
            try
            {
                if (await _depositmovementService.Edit(data, User.GetUserId()))
                    ShowSuccessMessage("El movimiento fue actualizado con éxito");
                else
                    ShowErrorMessage("Ocurrio un error al actualizar el movimiento, intentelo nuevamente");
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
        public async Task<IActionResult> AddProductToDepositMovement(DepositMovementData data)
        {
            ModelState.Clear();
            return PartialView("_Edit", await _depositmovementService.AddProductToDepositMovement(data));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepositMovement(DepositMovementData data)
        {
            ModelState.Clear();
            return PartialView("_Edit", data);
        }

        [HttpPost]
        public IActionResult DeleteProduct(DepositMovementData data, long productId)
        {
            ModelState.Clear();
            data.Products = data.Products.Where(p => p.ProductId != productId).ToList();
            return PartialView("_Edit", data);
        }

        public async Task<IActionResult> Detail(long id)
        {
            try
            {
                return View(await _depositmovementService.GetByIdForView(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DetectProducts(DepositMovementData data)
        {
            ModelState.Clear();

            if (data.Image == null || data.Image.Length == 0)
                return BadRequest("No se subió ninguna imagen");

            using var stream = data.Image.OpenReadStream();
            var detections = _productDetectorService.DetectProductsFromStream(stream);

            if (!detections.Any())
                return BadRequest("No se detecto ningún producto existente");

            return PartialView("_Edit", await _depositmovementService.AddProductsToMovement(data, detections.Select(d => d.ProductId).ToList()));
        }
    }
}
