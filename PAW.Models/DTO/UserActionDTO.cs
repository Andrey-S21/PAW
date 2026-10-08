using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserActionDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    public static UserActionDTO ConvertFrom(UserAction entity)
    {
        return new UserActionDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public static UserAction ConvertTo(UserActionDTO dto)
    {
        return new UserAction
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(UserAction entity)
    {
        entity.Name = Name;
        entity.Description = Description;
    }
}
