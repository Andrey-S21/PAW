using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ProductDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
    [JsonPropertyName("name")]
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("rating")]
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
    public int Rating { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }
    [JsonPropertyName("inventoryId")]
    public int? InventoryId { get; set; }
    [JsonPropertyName("supplierId")]
    public int? SupplierId { get; set; }
    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; set; }

    public static ProductDTO ConvertFrom(Product product)
    {
        return new ProductDTO
        {
            Id = Guid.NewGuid(),
            ProductId = product.ProductId,
            Name = product.ProductName!,
            Description = product.Description!,
            Rating = (int)(product.Rating ?? 0),
            ModifiedBy = product.ModifiedBy,
            CreatedBy = product.CreatedBy,
            Comments = string.Empty, // Assuming comments are not present in the Product entity
            CreatedDate = product.LastModified ?? DateTime.Now, // Assuming LastModified is used as CreatedDate
            ModifiedDate = product.LastModified ?? DateTime.Now, // Assuming LastModified is used as ModifiedDate
            InventoryId = product.InventoryId,
            SupplierId = product.SupplierId,
            CategoryId = product.CategoryId
        };
    }

    public static Product ConvertTo(ProductDTO productDTO)
    {
        return new Product
        {
            ProductId = productDTO.ProductId,
            ProductName = productDTO.Name,
            Description = productDTO.Description,
            Rating = productDTO.Rating,
            ModifiedBy = productDTO.ModifiedBy,
            CreatedBy = productDTO.CreatedBy,
            InventoryId = productDTO.InventoryId,
            SupplierId = productDTO.SupplierId,
            CategoryId = productDTO.CategoryId,
            LastModified = DateTime.Now
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada.</summary>
    public void ApplyTo(Product product)
    {
        product.ProductName = Name;
        product.Description = Description;
        product.Rating = Rating;
        product.ModifiedBy = ModifiedBy;
        product.CreatedBy = CreatedBy;
        product.InventoryId = InventoryId;
        product.SupplierId = SupplierId;
        product.CategoryId = CategoryId;
        product.LastModified = DateTime.Now;
    }
}
