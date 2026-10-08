using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserActionService
{
    Task<IEnumerable<UserActionDTO>> GetUserActionsAsync();
    Task<UserActionDTO> GetByIdAsync(int id);
    Task<UserActionDTO> CreateAsync(UserActionDTO dto);
    Task<UserActionDTO> UpdateAsync(UserActionDTO dto);
    Task DeleteAsync(int id);
}

public class UserActionService : ServiceBase, IUserActionService
{
    private const string _path = "UserAction";
    private readonly IRestProvider _restProvider;

    public UserActionService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserActionDTO>> GetUserActionsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<UserActionDTO>>(response);
        return items;
    }

    public async Task<UserActionDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<UserActionDTO>(response);
    }

    public async Task<UserActionDTO> CreateAsync(UserActionDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<UserActionDTO>(response);
    }

    public async Task<UserActionDTO> UpdateAsync(UserActionDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.Id}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<UserActionDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
