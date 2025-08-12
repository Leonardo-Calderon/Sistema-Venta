

namespace SVRepository.Entities
{
    /// <summary>
    /// Representa un rol de usuario en el sistema.
    /// </summary>
    /// <remarks>
    /// Los roles definen los permisos y accesos que tiene cada usuario en el sistema.
    /// Cada rol puede tener diferentes niveles de acceso a las funcionalidades
    /// y menús del sistema, implementando un control de acceso basado en roles (RBAC).
    /// </remarks>
    public class Rol
    {
        /// <summary>
        /// Identificador único del rol.
        /// </summary>
        /// <remarks>
        /// Este valor es generado automáticamente por la base de datos
        /// y se utiliza como clave primaria en la tabla de roles.
        /// </remarks>
        public int IdRol { get; set; }

        /// <summary>
        /// Nombre descriptivo del rol.
        /// </summary>
        /// <remarks>
        /// Nombre que identifica el rol y describe las responsabilidades
        /// y permisos asociados (ej: Administrador, Vendedor, Cajero, etc.).
        /// </remarks>
        public string Nombre { get; set; } = string.Empty;
    }
}
