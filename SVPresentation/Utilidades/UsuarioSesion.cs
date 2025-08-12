

namespace SVPresentation.Utilidades
{
    /// <summary>
    /// Clase estática que mantiene la información del usuario actualmente autenticado en la sesión.
    /// </summary>
    /// <remarks>
    /// Esta clase proporciona acceso global a la información del usuario que ha iniciado sesión
    /// en la aplicación. Se utiliza para controlar el acceso a funcionalidades específicas
    /// y personalizar la interfaz de usuario según el rol del usuario.
    /// </remarks>
    public static class UsuarioSesion
    {
        /// <summary>
        /// Identificador único del usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para identificar al usuario en las operaciones de base de datos
        /// y para el registro de auditoría de actividades.
        /// </remarks>
        public static int IdUsuario { get; set; }

        /// <summary>
        /// Nombre de usuario utilizado para el login.
        /// </summary>
        /// <remarks>
        /// Este es el nombre de usuario que se utiliza para autenticarse en el sistema.
        /// </remarks>
        public static string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Este valor se muestra en la interfaz de usuario para identificar al usuario
        /// que está utilizando la aplicación.
        /// </remarks>
        public static string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del rol asignado al usuario.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para determinar los permisos y accesos que tiene
        /// el usuario en el sistema.
        /// </remarks>
        public static int IdRol { get; set; }

        /// <summary>
        /// Nombre del rol asignado al usuario.
        /// </summary>
        /// <remarks>
        /// Este valor se muestra en la interfaz de usuario y se utiliza para
        /// personalizar la experiencia según el tipo de usuario.
        /// </remarks>
        public static string Rol { get; set; } = string.Empty;
    }
}
