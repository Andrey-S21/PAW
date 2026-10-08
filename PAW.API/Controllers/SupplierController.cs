using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController(ISupplierRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetSuppliers")]
        public async Task<IEnumerable<SupplierDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(SupplierDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetSupplierById")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return SupplierDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateSupplier")]
        public async Task<ActionResult<SupplierDTO>> Create([FromBody] SupplierDTO dto)
        {
            try
            {
                var entity = SupplierDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return SupplierDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateSupplier")]
        public async Task<ActionResult<SupplierDTO>> Update(int id, [FromBody] SupplierDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.SupplierId = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return SupplierDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteSupplier")]
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
