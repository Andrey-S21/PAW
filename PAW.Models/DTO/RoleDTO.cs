using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class RoleDTO
{
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
    [JsonPropertyName("roleName")]
    [Required(ErrorMessage = "Name is required.")]
    public string? RoleName { get; set; }

    public static RoleDTO ConvertFrom(Role entity)
    {
        return new RoleDTO
        {
            RoleId = entity.RoleId,
            RoleName = entity.RoleName
        };
    }

    public static Role ConvertTo(RoleDTO dto)
    {
        return new Role
        {
            RoleId = dto.RoleId,
            RoleName = dto.RoleName
        };
    }

    /// <summary>Copia los valores editables del DTO sobre una entidad ya cargada (conserva las columnas que el DTO no expone).</summary>
    public void ApplyTo(Role entity)
    {
        entity.RoleName = RoleName;
    }
}
