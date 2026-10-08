using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    Task<CategoryDTO> GetByIdAsync(int id);
    Task<CategoryDTO> CreateAsync(CategoryDTO dto);
    Task<CategoryDTO> UpdateAsync(CategoryDTO dto);
    Task DeleteAsync(int id);
}

public class CategoryService : ServiceBase, ICategoryService
{
    private const string _path = "Category";
    private readonly IRestProvider _restProvider;

    public CategoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<CategoryDTO>>(response);
        return items;
    }

    public async Task<CategoryDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<CategoryDTO>(response);
    }

    public async Task<CategoryDTO> CreateAsync(CategoryDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<CategoryDTO>(response);
    }

    public async Task<CategoryDTO> UpdateAsync(CategoryDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.CategoryId}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<CategoryDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
