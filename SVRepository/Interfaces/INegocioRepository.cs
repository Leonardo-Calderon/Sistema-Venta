
using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para la entidad Negocio.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Repository para la gestión de información del negocio,
    /// proporcionando métodos para obtener y actualizar los datos de configuración del negocio.
    /// Esta información incluye datos como razón social, RFC, dirección, contacto y configuración de moneda.
    /// </remarks>
    public interface INegocioRepository
    {
        /// <summary>
        /// Obtiene la información de configuración del negocio.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un objeto Negocio con la información del negocio.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna la información completa del negocio,
        /// incluyendo datos de contacto, dirección, configuración de moneda y logo.
        /// </remarks>
        Task<Negocio> Obtener();

        /// <summary>
        /// Actualiza la información de configuración del negocio.
        /// </summary>
        /// <param name="objeto">Objeto Negocio con los datos actualizados del negocio.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método actualiza todos los datos de configuración del negocio en la base de datos.
        /// Si ocurre un error durante la actualización, se lanza una excepción con el mensaje de error.
        /// </remarks>
        /// <exception cref="Exception">Se lanza cuando ocurre un error durante la actualización de los datos del negocio.</exception>
        Task Editar(Negocio objeto);
    }
}
