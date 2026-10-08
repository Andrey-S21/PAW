using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAW.Models.DTO;
using PAW.Web.Helpers;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IInventoryService inventoryService,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _productService.GetProductsAsync();
            return View(PagedList<ProductDTO>.Create(result, page));
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var details = await _productService.GetDetailsAsync(id);
                return PartialView("_Details", details);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Product {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                await LoadLookupsAsync();
                return PartialView("_Form", new ProductDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Product form");
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.CreateAsync(dto);
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Product");
                ModelState.AddModelError(string.Empty, ErrorMessageHelper.From(ex));
            }

            await LoadLookupsAsync();
            return PartialView("_Form", dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto = await _productService.GetByIdAsync(id);
                await LoadLookupsAsync();
                return PartialView("_Form", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Product {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductDTO dto)
        {
            dto.ProductId = id;
            // La ruta trae id=5 y ProductDTO.Id es un Guid: se ignora ese error de binding.
            ModelState.Remove(nameof(ProductDTO.Id));
            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.UpdateAsync(dto);
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Product {Id}", id);
                ModelState.AddModelError(string.Empty, ErrorMessageHelper.From(ex));
            }

            await LoadLookupsAsync();
            return PartialView("_Form", dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dto = await _productService.GetByIdAsync(id);
                return PartialView("_ConfirmDelete", new ConfirmDeleteViewModel
                {
                    EntityName = "Product",
                    Id = id.ToString(),
                    Label = dto.Name ?? $"#{id}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Product {Id}", id);
                return PartialView("_ModalError", ErrorMessageHelper.From(ex));
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Product {Id}", id);
                return PartialView("_ConfirmDelete", new ConfirmDeleteViewModel
                {
                    EntityName = "Product",
                    Id = id.ToString(),
                    Label = $"#{id}",
                    Error = ErrorMessageHelper.From(ex)
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>Carga las listas de categorías, suppliers e inventarios para los desplegables del formulario.</summary>
        private async Task LoadLookupsAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var suppliers = await _supplierService.GetSuppliersAsync();
            var inventories = await _inventoryService.GetInventoriesAsync();

            ViewBag.Categories = categories
                .Select(c => new SelectListItem(c.CategoryName ?? $"#{c.CategoryId}", c.CategoryId.ToString())).ToList();
            ViewBag.Suppliers = suppliers
                .Select(s => new SelectListItem(s.SupplierName ?? $"#{s.SupplierId}", s.SupplierId.ToString())).ToList();
            ViewBag.Inventories = inventories
                .Select(i => new SelectListItem($"Inventory #{i.InventoryId} ({i.UnitsInStock?.ToString() ?? "-"} in stock)", i.InventoryId.ToString())).ToList();
        }
    }
}
