using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class InventoryDTO
{
    [JsonPropertyName("inventoryId")]
    public int InventoryId { get; set; }
    [JsonPropertyName("unitPrice")]
    public decimal? UnitPrice { get; set; }
    [JsonPropertyName("unitsInStock")]
    public int? UnitsInStock { get; set; }
    [JsonPropertyName("lastUpdated")]
    public DateTime? LastUpdated { get; set; }
    [JsonPropertyName("productId")]
    public int? ProductId { get; set; }
    [JsonPropertyName("dateAdded")]
    public DateTime? DateAdded { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static InventoryDTO ConvertFrom(Inventory entity)
    {
        return new InventoryDTO
        {
            InventoryId = entity.InventoryId,
            UnitPrice = entity.UnitPrice,
            UnitsInStock = entity.UnitsInStock,
            LastUpdated = entity.LastUpdated,
            ProductId = entity.ProductId,
            DateAdded = entity.DateAdded,
            ModifiedBy = entity.ModifiedBy
        };
    }

    public static Inventory ConvertTo(InventoryDTO dto)
    {
        return new Inventory
        {
            InventoryId = dto.InventoryId,
            UnitPrice = dto.UnitPrice,
            UnitsInStock = dto.UnitsInStock,
            LastUpdated = dto.LastUpdated,
            ProductId = dto.ProductId,
            DateAdded = dto.DateAdded,
            ModifiedBy = dto.ModifiedBy
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(Inventory entity)
    {
        entity.UnitPrice = UnitPrice;
        entity.UnitsInStock = UnitsInStock;
        entity.LastUpdated = LastUpdated;
        entity.ProductId = ProductId;
        entity.DateAdded = DateAdded;
        entity.ModifiedBy = ModifiedBy;
    }
}
