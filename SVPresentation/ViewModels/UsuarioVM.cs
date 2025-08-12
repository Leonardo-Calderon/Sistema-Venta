using System.ComponentModel;

namespace SVPresentation.ViewModels
{
    /// <summary>
    /// ViewModel para la gestión de usuarios en la interfaz de usuario.
    /// </summary>
    /// <remarks>
    /// Esta clase representa los datos de un usuario que se muestran en la interfaz de usuario.
    /// Utiliza Data Annotations para definir cómo se muestran las propiedades en los controles
    /// de la interfaz, especialmente en DataGridViews y formularios de gestión de usuarios.
    /// </remarks>
    public class UsuarioVM
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        /// <remarks>
        /// Este es el identificador único del usuario en el sistema.
        /// </remarks>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Identificador del rol asignado al usuario.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para determinar los permisos y accesos que tiene
        /// el usuario en el sistema.
        /// </remarks>
        public int IdRol { get; set; }

        /// <summary>
        /// Nombre del rol asignado al usuario.
        /// </summary>
        /// <remarks>
        /// Este valor se muestra en la interfaz de usuario y se utiliza para
        /// personalizar la experiencia según el tipo de usuario.
        /// </remarks>
        public string Rol { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        /// <remarks>
        /// Esta propiedad se muestra como "Nombre Completo" en la interfaz de usuario
        /// gracias al atributo DisplayName.
        /// </remarks>
        [DisplayName("Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para la comunicación con el usuario y como
        /// identificador único en el sistema.
        /// </remarks>
        public string Correo { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de usuario utilizado para el login.
        /// </summary>
        /// <remarks>
        /// Esta propiedad se muestra como "Nombre Usuario" en la interfaz de usuario
        /// gracias al atributo DisplayName. Este es el nombre que se utiliza para
        /// autenticarse en el sistema.
        /// </remarks>
        [DisplayName("Nombre Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el usuario está activo en el sistema (1 = activo, 0 = inactivo).
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para controlar si el usuario puede acceder al sistema.
        /// </remarks>
        public int Activo { get; set; }

        /// <summary>
        /// Representación textual del estado de habilitación del usuario.
        /// </summary>
        /// <remarks>
        /// Este valor se muestra en la interfaz de usuario para indicar si el usuario
        /// está habilitado o deshabilitado de manera legible.
        /// </remarks>
        public string Habilitado { get; set; } = string.Empty;
    }
}
