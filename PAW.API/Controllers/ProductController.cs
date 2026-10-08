using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(
        IProductRepository repository,
        ICategoryRepository categoryRepository,
        ISupplierRepository supplierRepository,
        IInventoryRepository inventoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetProducts")]
        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(ProductDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return ProductDTO.ConvertFrom(entity);
        }

        /// <summary>Producto con su inventario, categoría y supplier asociados.</summary>
        [HttpGet("{id:int}/details", Name = "GetProductDetails")]
        public async Task<ActionResult<ProductDetailsDTO>> GetDetails(int id)
        {
            var product = await repository.FindAsync(id);
            if (product == null) return NotFound();

            Category? category = product.CategoryId is int categoryId ? await categoryRepository.FindAsync(categoryId) : null;
            Supplier? supplier = product.SupplierId is int supplierId ? await supplierRepository.FindAsync(supplierId) : null;
            Inventory? inventory = product.InventoryId is int inventoryId ? await inventoryRepository.FindAsync(inventoryId) : null;
            if (inventory == null)
            {
                var inventories = await inventoryRepository.ReadAsync() ?? [];
                inventory = inventories.FirstOrDefault(x => x.ProductId == id);
            }

            return new ProductDetailsDTO
            {
                Product = ProductDTO.ConvertFrom(product),
                Category = category == null ? null : CategoryDTO.ConvertFrom(category),
                Supplier = supplier == null ? null : SupplierDTO.ConvertFrom(supplier),
                Inventory = inventory == null ? null : InventoryDTO.ConvertFrom(inventory)
            };
        }

        [HttpPost(Name = "CreateProduct")]
        public async Task<ActionResult<ProductDTO>> Create([FromBody] ProductDTO dto)
        {
            try
            {
                var entity = ProductDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return ProductDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateProduct")]
        public async Task<ActionResult<ProductDTO>> Update(int id, [FromBody] ProductDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.ProductId = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return ProductDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                await repository.DeleteAsync(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }
    }
}
