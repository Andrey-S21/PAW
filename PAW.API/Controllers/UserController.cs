using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(UserDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return UserDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<ActionResult<UserDTO>> Create([FromBody] UserDTO dto)
        {
            try
            {
                var entity = UserDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return UserDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateUser")]
        public async Task<ActionResult<UserDTO>> Update(int id, [FromBody] UserDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.UserId = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return UserDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
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
