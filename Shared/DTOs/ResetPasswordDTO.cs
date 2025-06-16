namespace Shared.DTOs
{
    public class ResetPasswordDTO
    {
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
        public string Token { get; set; }
    }
}