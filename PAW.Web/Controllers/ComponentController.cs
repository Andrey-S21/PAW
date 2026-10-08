using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Helpers;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ComponentController : Controller
    {
        private readonly IComponentService _service;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(IComponentService service, ILogger<ComponentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var items = await _service.GetComponentsAsync();
            return View(PagedList<ComponentDTO>.Create(items, page));
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
                _logger.LogError(ex, "Error loading Component {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Form", new ComponentDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComponentDTO dto)
        {
            if (!ModelState.IsValid) return PartialView("_Form", dto);

            try
            {
                await _service.CreateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Component");
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
                _logger.LogError(ex, "Error loading Component {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ComponentDTO dto)
        {
            dto.Id = id;
            if (!ModelState.IsValid) return PartialView("_Form", dto);

            try
            {
                await _service.UpdateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Component {Id}", id);
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
                    EntityName = "Component",
                    Id = id.ToString(),
                    Label = dto.Name ?? $"#{id}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Component {Id}", id);
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
                _logger.LogError(ex, "Error deleting Component {Id}", id);
                return PartialView("_ConfirmDelete", new ConfirmDeleteViewModel
                {
                    EntityName = "Component",
                    Id = id.ToString(),
                    Label = $"#{id}",
                    Error = ErrorMessageHelper.From(ex)
                });
            }
        }
    }
}
