using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PawTask = PAW.Models.Task;

namespace PAW.Models.DTO;

public class PawTaskDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static PawTaskDTO ConvertFrom(PawTask entity)
    {
        return new PawTaskDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Status = entity.Status,
            DueDate = entity.DueDate,
            CreatedAt = entity.CreatedAt,
            LastModified = entity.LastModified,
            ModifiedBy = entity.ModifiedBy
        };
    }

    public static PawTask ConvertTo(PawTaskDTO dto)
    {
        return new PawTask
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate,
            CreatedAt = dto.CreatedAt,
            LastModified = dto.LastModified,
            ModifiedBy = dto.ModifiedBy
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(PawTask entity)
    {
        entity.Name = Name;
        entity.Description = Description;
        entity.Status = Status;
        entity.DueDate = DueDate;
        entity.CreatedAt = CreatedAt;
        entity.LastModified = LastModified;
        entity.ModifiedBy = ModifiedBy;
    }
}
