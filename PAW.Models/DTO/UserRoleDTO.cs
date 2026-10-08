using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserRoleDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("roldId")]
    public decimal? RoldId { get; set; }
    [JsonPropertyName("userId")]
    public decimal? UserId { get; set; }

    public static UserRoleDTO ConvertFrom(UserRole entity)
    {
        return new UserRoleDTO
        {
            Id = entity.Id,
            RoldId = entity.RoldId,
            UserId = entity.UserId
        };
    }

    public static UserRole ConvertTo(UserRoleDTO dto)
    {
        return new UserRole
        {
            Id = dto.Id,
            RoldId = dto.RoldId,
            UserId = dto.UserId
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(UserRole entity)
    {
        entity.RoldId = RoldId;
        entity.UserId = UserId;
    }
}
