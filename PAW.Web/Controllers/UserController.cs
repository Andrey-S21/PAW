using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Helpers;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var items = await _service.GetUsersAsync();
            return View(PagedList<UserDTO>.Create(items, page));
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
                _logger.LogError(ex, "Error loading User {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Form", new UserDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            if (!ModelState.IsValid) return PartialView("_Form", dto);

            try
            {
                dto.CreatedAt ??= DateTime.Now;
                dto.LastModified = DateTime.Now;
                await _service.CreateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating User");
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
                _logger.LogError(ex, "Error loading User {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserDTO dto)
        {
            dto.UserId = id;
            if (!ModelState.IsValid) return PartialView("_Form", dto);

            try
            {
                dto.LastModified = DateTime.Now;
                await _service.UpdateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating User {Id}", id);
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
                    EntityName = "User",
                    Id = id.ToString(),
                    Label = dto.Username ?? $"#{id}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading User {Id}", id);
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
                _logger.LogError(ex, "Error deleting User {Id}", id);
                return PartialView("_ConfirmDelete", new ConfirmDeleteViewModel
                {
                    EntityName = "User",
                    Id = id.ToString(),
                    Label = $"#{id}",
                    Error = ErrorMessageHelper.From(ex)
                });
            }
        }
    }
}
