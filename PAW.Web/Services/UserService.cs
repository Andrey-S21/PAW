using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsersAsync();
    Task<UserDTO> GetByIdAsync(int id);
    Task<UserDTO> CreateAsync(UserDTO dto);
    Task<UserDTO> UpdateAsync(UserDTO dto);
    Task DeleteAsync(int id);
}

public class UserService : ServiceBase, IUserService
{
    private const string _path = "User";
    private readonly IRestProvider _restProvider;

    public UserService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<UserDTO>>(response);
        return items;
    }

    public async Task<UserDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<UserDTO>(response);
    }

    public async Task<UserDTO> CreateAsync(UserDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<UserDTO>(response);
    }

    public async Task<UserDTO> UpdateAsync(UserDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.UserId}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<UserDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
