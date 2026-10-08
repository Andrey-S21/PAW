using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Helpers;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _service;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService service, ILogger<InventoryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var items = await _service.GetInventoriesAsync();
            return View(PagedList<InventoryDTO>.Create(items, page));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                return PartialView("_Details", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Inventory {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Form", new InventoryDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryDTO dto)
        {
            if (!ModelState.IsValid) return PartialView("_Form", dto);

            try
            {
                dto.LastUpdated = DateTime.Now;
                dto.DateAdded ??= DateTime.Now;
                await _service.CreateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Inventory");
                ModelState.AddModelError(string.Empty, ErrorMessageHelper.From(ex));
                return PartialView("_Form", dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                return PartialView("_Form", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Inventory {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, InventoryDTO dto)
        {
            dto.InventoryId = id;
            if (!ModelState.IsValid) return PartialView("_Form", dto);

            try
            {
                dto.LastUpdated = DateTime.Now;
                await _service.UpdateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Inventory {Id}", id);
                ModelState.AddModelError(string.Empty, ErrorMessageHelper.From(ex));
                return PartialView("_Form", dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                return PartialView("_ConfirmDelete", new ConfirmDeleteViewModel
                {
                    EntityName = "Inventory",
                    Id = id.ToString(),
                    Label = $"#{dto.InventoryId}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Inventory {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Inventory {Id}", id);
                return PartialView("_ConfirmDelete", new ConfirmDeleteViewModel
                {
                    EntityName = "Inventory",
                    Id = id.ToString(),
                    Label = $"#{id}",
                    Error = ErrorMessageHelper.From(ex)
                });
            }
        }
    }
}
