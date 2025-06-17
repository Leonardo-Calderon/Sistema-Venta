namespace Shared.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }
        public string? NombreUsuario { get; set; }
        public int IdRol { get; set; }
        public string? DescripcionRol { get; set; } // Nombre del rol para mostrar
        public bool Activo { get; set; }
    }
}