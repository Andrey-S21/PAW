using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Helpers;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService service, ILogger<CategoryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var items = await _service.GetCategoriesAsync();
            return View(PagedList<CategoryDTO>.Create(items, page));
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
                _logger.LogError(ex, "Error loading Category {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Form", new CategoryDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            if (!ModelState.IsValid) return PartialView("_Form", dto);

            try
            {
                dto.LastModified = DateTime.Now;
                await _service.CreateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Category");
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
                _logger.LogError(ex, "Error loading Category {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryDTO dto)
        {
            dto.CategoryId = id;
            if (!ModelState.IsValid) return PartialView("_Form", dto);

            try
            {
                dto.LastModified = DateTime.Now;
                await _service.UpdateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Category {Id}", id);
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
                    EntityName = "Category",
                    Id = id.ToString(),
                    Label = dto.CategoryName ?? $"#{id}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Category {Id}", id);
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
                _logger.LogError(ex, "Error deleting Category {Id}", id);
                return PartialView("_ConfirmDelete", new ConfirmDeleteViewModel
                {
                    EntityName = "Category",
                    Id = id.ToString(),
                    Label = $"#{id}",
                    Error = ErrorMessageHelper.From(ex)
                });
            }
        }
    }
}
