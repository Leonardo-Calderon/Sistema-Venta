namespace Shared.DTOs
{
    public class UsuarioCreadoResponseDTO
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string ContrasenaTemporal { get; set; }
        public bool EsContrasenaTemporal { get; set; }
        public string Mensaje { get; set; }
    }
}
