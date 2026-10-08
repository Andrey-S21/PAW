using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class NotificationDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("message")]
    [Required(ErrorMessage = "Message is required.")]
    public string? Message { get; set; }
    [JsonPropertyName("isRead")]
    public bool? IsRead { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    public static NotificationDTO ConvertFrom(Notification entity)
    {
        return new NotificationDTO
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Message = entity.Message,
            IsRead = entity.IsRead,
            CreatedAt = entity.CreatedAt
        };
    }

    public static Notification ConvertTo(NotificationDTO dto)
    {
        return new Notification
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Message = dto.Message!,
            IsRead = dto.IsRead,
            CreatedAt = dto.CreatedAt
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(Notification entity)
    {
        entity.UserId = UserId;
        entity.Message = Message!;
        entity.IsRead = IsRead;
        entity.CreatedAt = CreatedAt;
    }
}
