using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesAsync();
    Task<RoleDTO> GetByIdAsync(int id);
    Task<RoleDTO> CreateAsync(RoleDTO dto);
    Task<RoleDTO> UpdateAsync(RoleDTO dto);
    Task DeleteAsync(int id);
}

public class RoleService : ServiceBase, IRoleService
{
    private const string _path = "Role";
    private readonly IRestProvider _restProvider;

    public RoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<RoleDTO>>(response);
        return items;
    }

    public async Task<RoleDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<RoleDTO>(response);
    }

    public async Task<RoleDTO> CreateAsync(RoleDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<RoleDTO>(response);
    }

    public async Task<RoleDTO> UpdateAsync(RoleDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.RoleId}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<RoleDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
