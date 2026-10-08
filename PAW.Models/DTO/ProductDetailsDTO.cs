using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

/// <summary>Producto con su inventario, categoría y supplier asociados.</summary>
public class ProductDetailsDTO
{
    [JsonPropertyName("product")]
    public ProductDTO Product { get; set; } = new();
    [JsonPropertyName("inventory")]
    public InventoryDTO? Inventory { get; set; }
    [JsonPropertyName("category")]
    public CategoryDTO? Category { get; set; }
    [JsonPropertyName("supplier")]
    public SupplierDTO? Supplier { get; set; }
}
