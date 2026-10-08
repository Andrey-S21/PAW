using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ICategoryRepository repository) : ControllerBase
    {
        [HttpGet(Name = "GetCategories")]
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var items = await repository.ReadAsync() ?? [];
            return items.Select(CategoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();
            return CategoryDTO.ConvertFrom(entity);
        }

        [HttpPost(Name = "CreateCategory")]
        public async Task<ActionResult<CategoryDTO>> Create([FromBody] CategoryDTO dto)
        {
            try
            {
                var entity = CategoryDTO.ConvertTo(dto);
                await repository.CreateAsync(entity);
                return CategoryDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateCategory")]
        public async Task<ActionResult<CategoryDTO>> Update(int id, [FromBody] CategoryDTO dto)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null) return NotFound();

            try
            {
                dto.CategoryId = id;
                dto.ApplyTo(entity);
                await repository.UpdateAsync(entity);
                return CategoryDTO.ConvertFrom(entity);
            }
            catch (Exception ex)
            {
                return Problem(detail: ErrorHelper.Message(ex), statusCode: StatusCodes.Status409Conflict);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteCategory")]
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
