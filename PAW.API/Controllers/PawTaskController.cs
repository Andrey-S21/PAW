using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PawTaskController(IPawTaskRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetPawTasks")]
        public async Task<IEnumerable<PawTaskDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(PawTaskDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetPawTaskById")]
        public async Task<ActionResult<PawTaskDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return PawTaskDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreatePawTask")]
        public async Task<ActionResult<PawTaskDTO>> Create([FromBody] PawTaskDTO dto)
        {
            try
            {
                var entity = PawTaskDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return PawTaskDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdatePawTask")]
        public async Task<ActionResult<PawTaskDTO>> Update(int id, [FromBody] PawTaskDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.Id = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return PawTaskDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePawTask")]
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
