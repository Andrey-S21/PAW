using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class SupplierDTO
{
    [JsonPropertyName("supplierId")]
    public int SupplierId { get; set; }
    [JsonPropertyName("supplierName")]
    [Required(ErrorMessage = "Name is required.")]
    public string? SupplierName { get; set; }
    [JsonPropertyName("contactName")]
    public string? ContactName { get; set; }
    [JsonPropertyName("contactTitle")]
    public string? ContactTitle { get; set; }
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [JsonPropertyName("city")]
    public string? City { get; set; }
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static SupplierDTO ConvertFrom(Supplier entity)
    {
        return new SupplierDTO
        {
            SupplierId = entity.SupplierId,
            SupplierName = entity.SupplierName,
            ContactName = entity.ContactName,
            ContactTitle = entity.ContactTitle,
            Phone = entity.Phone,
            Address = entity.Address,
            City = entity.City,
            Country = entity.Country,
            LastModified = entity.LastModified,
            ModifiedBy = entity.ModifiedBy
        };
    }

    public static Supplier ConvertTo(SupplierDTO dto)
    {
        return new Supplier
        {
            SupplierId = dto.SupplierId,
            SupplierName = dto.SupplierName,
            ContactName = dto.ContactName,
            ContactTitle = dto.ContactTitle,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country,
            LastModified = dto.LastModified,
            ModifiedBy = dto.ModifiedBy
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(Supplier entity)
    {
        entity.SupplierName = SupplierName;
        entity.ContactName = ContactName;
        entity.ContactTitle = ContactTitle;
        entity.Phone = Phone;
        entity.Address = Address;
        entity.City = City;
        entity.Country = Country;
        entity.LastModified = LastModified;
        entity.ModifiedBy = ModifiedBy;
    }
}
