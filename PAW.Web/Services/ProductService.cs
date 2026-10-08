using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync();
    Task<ProductDTO> GetByIdAsync(int id);
    Task<ProductDTO> CreateAsync(ProductDTO dto);
    Task<ProductDTO> UpdateAsync(ProductDTO dto);
    Task DeleteAsync(int id);
    Task<ProductDetailsDTO> GetDetailsAsync(int id);
}

public class ProductService : ServiceBase, IProductService
{
    private const string _path = "Product";
    private readonly IRestProvider _restProvider;

    public ProductService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<ProductDTO>>(response);
        return items;
    }

    public async Task<ProductDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<ProductDTO>(response);
    }

    public async Task<ProductDTO> CreateAsync(ProductDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<ProductDTO>(response);
    }

    public async Task<ProductDTO> UpdateAsync(ProductDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.ProductId}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<ProductDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }

    public async Task<ProductDetailsDTO> GetDetailsAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}/details"), id: null);
        return await JsonProvider.DeserializeAsync<ProductDetailsDTO>(response);
    }
}
