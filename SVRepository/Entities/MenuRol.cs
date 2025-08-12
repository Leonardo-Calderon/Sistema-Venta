namespace SVRepository.Entities
{
    /// <summary>
    /// Representa un menú del sistema asociado a un rol específico.
    /// </summary>
    /// <remarks>
    /// Esta entidad implementa el patrón de control de acceso basado en roles (RBAC),
    /// permitiendo definir qué menús y funcionalidades están disponibles para cada rol
    /// de usuario en el sistema.
    /// </remarks>
    public class MenuRol
    {
        /// <summary>
        /// Identificador único del menú.
        /// </summary>
        /// <remarks>
        /// Este valor identifica de forma única cada menú en el sistema
        /// y se utiliza para establecer las relaciones de permisos con los roles.
        /// </remarks>
        public int IdMenu { get; set; }

        /// <summary>
        /// Nombre descriptivo del menú.
        /// </summary>
        /// <remarks>
        /// Este campo contiene el nombre que se mostrará en la interfaz de usuario
        /// para identificar la funcionalidad o sección del sistema.
        /// </remarks>
        public string NombreMenu { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del menú padre, si existe.
        /// </summary>
        /// <remarks>
        /// Permite crear una estructura jerárquica de menús (menús y submenús).
        /// Si el valor es 0, indica que es un menú principal sin padre.
        /// </remarks>
        public int IdMenuPadre { get; set; }

        /// <summary>
        /// Indica si el menú está activo y disponible para el rol.
        /// </summary>
        /// <remarks>
        /// Controla si el menú es visible y accesible para los usuarios
        /// que tienen asignado el rol correspondiente.
        /// </remarks>
        public bool Activo { get; set; }
    }
}