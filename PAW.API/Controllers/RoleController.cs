using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(IRoleRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetRoles")]
        public async Task<IEnumerable<RoleDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(RoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetRoleById")]
        public async Task<ActionResult<RoleDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return RoleDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateRole")]
        public async Task<ActionResult<RoleDTO>> Create([FromBody] RoleDTO dto)
        {
            try
            {
                var entity = RoleDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return RoleDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateRole")]
        public async Task<ActionResult<RoleDTO>> Update(int id, [FromBody] RoleDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.RoleId = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return RoleDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteRole")]
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
