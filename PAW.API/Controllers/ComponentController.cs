using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController(IComponentRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetComponents")]
        public async Task<IEnumerable<ComponentDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(ComponentDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return ComponentDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateComponent")]
        public async Task<ActionResult<ComponentDTO>> Create([FromBody] ComponentDTO dto)
        {
            try
            {
                var entity = ComponentDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return ComponentDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateComponent")]
        public async Task<ActionResult<ComponentDTO>> Update(int id, [FromBody] ComponentDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.Id = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return ComponentDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteComponent")]
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
