using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IComponentService
{
    Task<IEnumerable<ComponentDTO>> GetComponentsAsync();
    Task<ComponentDTO> GetByIdAsync(int id);
    Task<ComponentDTO> CreateAsync(ComponentDTO dto);
    Task<ComponentDTO> UpdateAsync(ComponentDTO dto);
    Task DeleteAsync(int id);
}

public class ComponentService : ServiceBase, IComponentService
{
    private const string _path = "Component";
    private readonly IRestProvider _restProvider;

    public ComponentService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ComponentDTO>> GetComponentsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<ComponentDTO>>(response);
        return items;
    }

    public async Task<ComponentDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<ComponentDTO>(response);
    }

    public async Task<ComponentDTO> CreateAsync(ComponentDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<ComponentDTO>(response);
    }

    public async Task<ComponentDTO> UpdateAsync(ComponentDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.Id}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<ComponentDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
