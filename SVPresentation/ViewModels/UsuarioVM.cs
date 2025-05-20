using System;
using System.ComponentModel;
namespace SVPresentation.ViewModels
{
    public class UsuarioVM
    {
        public int IdUsuario { get; set; }
        public int IdRol {  get; set; }
        public string Rol { get; set; }
        [DisplayName("Nombre Completo")]
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        [DisplayName("Nombre Usuario")]
        public string NombreUsuario { get; set; }
        public int Activo { get; set; }
        public string Habilitado { get; set; }
    }
}
