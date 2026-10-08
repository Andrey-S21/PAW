using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ComponentDTO
{
    [JsonPropertyName("id")]
    public decimal Id { get; set; }
    [JsonPropertyName("name")]
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    [JsonPropertyName("content")]
    [Required(ErrorMessage = "Content is required.")]
    public string? Content { get; set; }

    public static ComponentDTO ConvertFrom(Component entity)
    {
        return new ComponentDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Content = entity.Content
        };
    }

    public static Component ConvertTo(ComponentDTO dto)
    {
        return new Component
        {
            Id = dto.Id,
            Name = dto.Name!,
            Content = dto.Content!
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(Component entity)
    {
        entity.Name = Name!;
        entity.Content = Content!;
    }
}
