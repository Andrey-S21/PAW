using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserActionController(IUserActionRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetUserActions")]
        public async Task<IEnumerable<UserActionDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(UserActionDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserActionById")]
        public async Task<ActionResult<UserActionDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return UserActionDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateUserAction")]
        public async Task<ActionResult<UserActionDTO>> Create([FromBody] UserActionDTO dto)
        {
            try
            {
                var existing = await repository.ReadAsync() ?? [];
                dto.Id = existing.Any() ? existing.Max(x => x.Id) + 1 : 1;
                var entity = UserActionDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return UserActionDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateUserAction")]
        public async Task<ActionResult<UserActionDTO>> Update(int id, [FromBody] UserActionDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.Id = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return UserActionDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteUserAction")]
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
