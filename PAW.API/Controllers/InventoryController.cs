using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController(IInventoryRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetInventories")]
        public async Task<IEnumerable<InventoryDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(InventoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetInventoryById")]
        public async Task<ActionResult<InventoryDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return InventoryDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateInventory")]
        public async Task<ActionResult<InventoryDTO>> Create([FromBody] InventoryDTO dto)
        {
            try
            {
                var entity = InventoryDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return InventoryDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateInventory")]
        public async Task<ActionResult<InventoryDTO>> Update(int id, [FromBody] InventoryDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.InventoryId = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return InventoryDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteInventory")]
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
