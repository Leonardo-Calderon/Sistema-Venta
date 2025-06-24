using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs
{
    public class LoginDTO
    {
        [JsonPropertyName("nombreUsuario")] // Forzar nombre en minúsculas
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string? NombreUsuario { get; set; }

        [JsonPropertyName("clave")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Clave { get; set; }
    }
}