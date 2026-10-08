using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController(IUserRoleRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetUserRoles")]
        public async Task<IEnumerable<UserRoleDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(UserRoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserRoleById")]
        public async Task<ActionResult<UserRoleDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return UserRoleDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateUserRole")]
        public async Task<ActionResult<UserRoleDTO>> Create([FromBody] UserRoleDTO dto)
        {
            try
            {
                var existing = await repository.ReadAsync() ?? [];
                dto.Id = existing.Any() ? existing.Max(x => x.Id) + 1 : 1;
                var entity = UserRoleDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return UserRoleDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateUserRole")]
        public async Task<ActionResult<UserRoleDTO>> Update(int id, [FromBody] UserRoleDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.Id = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return UserRoleDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteUserRole")]
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
