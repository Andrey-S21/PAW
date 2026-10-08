using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawTaskService
{
    Task<IEnumerable<PawTaskDTO>> GetPawTasksAsync();
    Task<PawTaskDTO> GetByIdAsync(int id);
    Task<PawTaskDTO> CreateAsync(PawTaskDTO dto);
    Task<PawTaskDTO> UpdateAsync(PawTaskDTO dto);
    Task DeleteAsync(int id);
}

public class PawTaskService : ServiceBase, IPawTaskService
{
    private const string _path = "PawTask";
    private readonly IRestProvider _restProvider;

    public PawTaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<PawTaskDTO>> GetPawTasksAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<PawTaskDTO>>(response);
        return items;
    }

    public async Task<PawTaskDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<PawTaskDTO>(response);
    }

    public async Task<PawTaskDTO> CreateAsync(PawTaskDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<PawTaskDTO>(response);
    }

    public async Task<PawTaskDTO> UpdateAsync(PawTaskDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.Id}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<PawTaskDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
