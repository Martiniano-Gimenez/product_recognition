using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Data;
using Service.ServiceContracts;
using ShoppingCart.Controllers.Base;

namespace ShoppingCart.Controllers
{
    public class ClientController : BaseController
    {
        private readonly IClientService _clientService;
        private readonly ISellerService _sellerService;

        public ClientController(IClientService clientService,
                                ISellerService sellerService)
        {
            _clientService = clientService;
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grid([FromBody] DTParameters param)
        {
            GridData<ClientGridData> gridData = await _clientService.GetAllPaginated(param);
            return new JsonResult(new { draw = param.Draw++, recordsTotal = gridData.Count, recordsFiltered = gridData.Count, data = gridData.List });
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Sellers = await _sellerService.GetAllSelectable();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientData data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _clientService.Create(data))
                        ShowSuccessMessage("El cliente fue creado con éxito");
                    else
                        ShowErrorMessage("Ocurrio un error al crear el cliente, intentelo nuevamente");
                    return RedirectToAction("Index");
                }
                ViewBag.Sellers = await _sellerService.GetAllSelectable();
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
                ViewBag.Sellers = await _sellerService.GetAllSelectable();
                return View(await _clientService.GetById(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClientData data)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    if (await _clientService.Edit(data))
                        ShowSuccessMessage("El cliente fue actualizado con éxito");
                    else
                        ShowErrorMessage("Ocurrio un error al actualizar el cliente, intentelo nuevamente");
                    return RedirectToAction("Index");
                }
                ViewBag.Sellers = await _sellerService.GetAllSelectable();
                return View(data);
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Detail(long id)
        {
            try
            {
                return View(await _clientService.GetViewById(id));
            }
            catch (BusinessException ex)
            {
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}
