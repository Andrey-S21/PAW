using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDTO>> GetInventoriesAsync();
    Task<InventoryDTO> GetByIdAsync(int id);
    Task<InventoryDTO> CreateAsync(InventoryDTO dto);
    Task<InventoryDTO> UpdateAsync(InventoryDTO dto);
    Task DeleteAsync(int id);
}

public class InventoryService : ServiceBase, IInventoryService
{
    private const string _path = "Inventory";
    private readonly IRestProvider _restProvider;

    public InventoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<InventoryDTO>> GetInventoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<InventoryDTO>>(response);
        return items;
    }

    public async Task<InventoryDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<InventoryDTO>(response);
    }

    public async Task<InventoryDTO> CreateAsync(InventoryDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<InventoryDTO>(response);
    }

    public async Task<InventoryDTO> UpdateAsync(InventoryDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.InventoryId}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<InventoryDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
