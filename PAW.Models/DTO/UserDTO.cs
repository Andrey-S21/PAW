using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserDTO
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("username")]
    [Required(ErrorMessage = "Username is required.")]
    public string? Username { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("isActive")]
    public bool? IsActive { get; set; }
    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("roleId")]
    public int? RoleId { get; set; }
    [JsonPropertyName("lastModifiedBy")]
    public string? LastModifiedBy { get; set; }

    public static UserDTO ConvertFrom(User entity)
    {
        return new UserDTO
        {
            UserId = entity.UserId,
            Username = entity.Username,
            Email = entity.Email,
            CreatedAt = entity.CreatedAt,
            IsActive = entity.IsActive,
            LastModified = entity.LastModified,
            ModifiedBy = entity.ModifiedBy,
            RoleId = entity.RoleId,
            LastModifiedBy = entity.LastModifiedBy
        };
    }

    public static User ConvertTo(UserDTO dto)
    {
        return new User
        {
            UserId = dto.UserId,
            Username = dto.Username,
            Email = dto.Email,
            CreatedAt = dto.CreatedAt,
            IsActive = dto.IsActive,
            LastModified = dto.LastModified,
            ModifiedBy = dto.ModifiedBy,
            RoleId = dto.RoleId,
            LastModifiedBy = dto.LastModifiedBy,
            PasswordHash = string.Empty
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(User entity)
    {
        entity.Username = Username;
        entity.Email = Email;
        entity.CreatedAt = CreatedAt;
        entity.IsActive = IsActive;
        entity.LastModified = LastModified;
        entity.ModifiedBy = ModifiedBy;
        entity.RoleId = RoleId;
        entity.LastModifiedBy = LastModifiedBy;
    }
}
