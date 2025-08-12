using SVRepository.Entities;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de información del negocio.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la gestión de información del negocio,
    /// proporcionando una capa de abstracción entre los controladores y los repositorios.
    /// La información del negocio incluye datos fiscales, de contacto y configuración del sistema.
    /// </remarks>
    public interface INegocioService
    {
        /// <summary>
        /// Obtiene la información de configuración del negocio.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un objeto Negocio con la información del negocio.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<Negocio> Obtener();

        /// <summary>
        /// Actualiza la información de configuración del negocio.
        /// </summary>
        /// <param name="objeto">Objeto Negocio con los datos actualizados del negocio.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de actualizar,
        /// como verificar que los datos son válidos y cumplen con las reglas de negocio.
        /// </remarks>
        Task Editar(Negocio objeto);
    }
}
