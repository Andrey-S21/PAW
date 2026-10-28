using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IMinimalTaskService
{
    Task<IEnumerable<PawTaskDTO>> GetTasksAsync();
    Task<PawTaskDTO> GetByIdAsync(int id);
    Task<PawTaskDTO> CreateAsync(PawTaskDTO dto);
    Task UpdateAsync(PawTaskDTO dto);
    Task DeleteAsync(int id);
}

public class MinimalTaskService : ServiceBase, IMinimalTaskService
{
    private const string _path = "tasks";
    private readonly IRestProvider _restProvider;

    public MinimalTaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
        BaseUrl = "https://localhost:7200/"; 
    }

    public async Task<IEnumerable<PawTaskDTO>> GetTasksAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        return await JsonProvider.DeserializeAsync<IEnumerable<PawTaskDTO>>(response);
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

    // El minimal API responde 204 No Content, no hay cuerpo que leer
    public async Task UpdateAsync(PawTaskDTO dto)
    {
        await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.Id}"), string.Empty, JsonProvider.Serialize(dto));
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}