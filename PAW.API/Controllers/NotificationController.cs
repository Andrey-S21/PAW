using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(INotificationRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetNotifications")]
        public async Task<IEnumerable<NotificationDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(NotificationDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetNotificationById")]
        public async Task<ActionResult<NotificationDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return NotificationDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateNotification")]
        public async Task<ActionResult<NotificationDTO>> Create([FromBody] NotificationDTO dto)
        {
            try
            {
                var entity = NotificationDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return NotificationDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateNotification")]
        public async Task<ActionResult<NotificationDTO>> Update(int id, [FromBody] NotificationDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.Id = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return NotificationDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteNotification")]
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
