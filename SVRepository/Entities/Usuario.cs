
namespace SVRepository.Entities
{
    /// <summary>
    /// Representa un usuario del sistema de ventas.
    /// </summary>
    /// <remarks>
    /// Los usuarios son las personas que acceden al sistema y realizan operaciones
    /// como ventas, gestión de productos, reportes, etc. Cada usuario tiene un rol
    /// que define sus permisos y accesos en el sistema.
    /// </remarks>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        /// <remarks>
        /// Este valor es generado automáticamente por la base de datos
        /// y se utiliza como clave primaria en la tabla de usuarios.
        /// </remarks>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Rol asignado al usuario.
        /// </summary>
        /// <remarks>
        /// Define los permisos y accesos que tiene el usuario en el sistema.
        /// El rol determina qué funcionalidades y menús están disponibles.
        /// </remarks>
        public Rol RefRol { get; set; } = new Rol();

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        /// <remarks>
        /// Nombre completo de la persona que utiliza el sistema.
        /// Se utiliza para identificación y en reportes de actividad.
        /// </remarks>
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// </summary>
        /// <remarks>
        /// Email único del usuario utilizado para comunicación,
        /// recuperación de contraseña y notificaciones del sistema.
        /// </remarks>
        public string Correo { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de usuario para el acceso al sistema.
        /// </summary>
        /// <remarks>
        /// Identificador único utilizado para el inicio de sesión.
        /// Debe ser único en el sistema y no puede contener espacios.
        /// </remarks>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        /// <remarks>
        /// Contraseña encriptada utilizada para autenticar al usuario.
        /// Se almacena de forma segura en la base de datos.
        /// </remarks>
        public string Clave { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el usuario debe cambiar su contraseña en el próximo inicio de sesión.
        /// </summary>
        /// <remarks>
        /// Valores posibles:
        /// - 1: Usuario debe cambiar contraseña
        /// - 0: Usuario no necesita cambiar contraseña
        /// Se utiliza para forzar el cambio de contraseña por seguridad.
        /// </remarks>
        public int ResetearClave { get; set; }

        /// <summary>
        /// Indica si el usuario está activo en el sistema.
        /// </summary>
        /// <remarks>
        /// Valores posibles:
        /// - 1: Usuario activo (puede acceder al sistema)
        /// - 0: Usuario inactivo (no puede acceder al sistema)
        /// Se utiliza para deshabilitar usuarios sin eliminarlos.
        /// </remarks>
        public int Activo { get; set; }
    }
}
