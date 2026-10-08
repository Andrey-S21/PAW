using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserRoleService
{
    Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync();
    Task<UserRoleDTO> GetByIdAsync(int id);
    Task<UserRoleDTO> CreateAsync(UserRoleDTO dto);
    Task<UserRoleDTO> UpdateAsync(UserRoleDTO dto);
    Task DeleteAsync(int id);
}

public class UserRoleService : ServiceBase, IUserRoleService
{
    private const string _path = "UserRole";
    private readonly IRestProvider _restProvider;

    public UserRoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<UserRoleDTO>>(response);
        return items;
    }

    public async Task<UserRoleDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<UserRoleDTO>(response);
    }

    public async Task<UserRoleDTO> CreateAsync(UserRoleDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<UserRoleDTO>(response);
    }

    public async Task<UserRoleDTO> UpdateAsync(UserRoleDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.Id}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<UserRoleDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
