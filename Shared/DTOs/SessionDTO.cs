// En Shared/DTOs/SessionDTO.cs
namespace Shared.DTOs
{
    public class SessionDTO
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
    }
}