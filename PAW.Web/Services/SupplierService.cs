using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();
    Task<SupplierDTO> GetByIdAsync(int id);
    Task<SupplierDTO> CreateAsync(SupplierDTO dto);
    Task<SupplierDTO> UpdateAsync(SupplierDTO dto);
    Task DeleteAsync(int id);
}

public class SupplierService : ServiceBase, ISupplierService
{
    private const string _path = "Supplier";
    private readonly IRestProvider _restProvider;

    public SupplierService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<SupplierDTO>> GetSuppliersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<SupplierDTO>>(response);
        return items;
    }

    public async Task<SupplierDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<SupplierDTO>(response);
    }

    public async Task<SupplierDTO> CreateAsync(SupplierDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<SupplierDTO>(response);
    }

    public async Task<SupplierDTO> UpdateAsync(SupplierDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.SupplierId}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<SupplierDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
