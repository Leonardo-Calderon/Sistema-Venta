using SVRepository.Entities;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de categorías de productos.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la gestión de categorías, proporcionando
    /// una capa de abstracción entre los controladores y los repositorios. Los servicios
    /// pueden incluir lógica de negocio, validaciones y transformaciones de datos.
    /// </remarks>
    public interface ICategoriaService
    {
        /// <summary>
        /// Obtiene una lista de categorías opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar las categorías por nombre. Si está vacío, retorna todas las categorías.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de categorías.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<List<Categoria>> Lista(string buscar = "");

        /// <summary>
        /// Crea una nueva categoría en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Categoria con los datos de la nueva categoría a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de delegar la operación
        /// al repositorio, como verificar duplicados o validar reglas de negocio.
        /// </remarks>
        Task<string> Crear(Categoria objeto);

        /// <summary>
        /// Actualiza los datos de una categoría existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Categoria con los datos actualizados de la categoría.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de actualizar,
        /// como verificar que la categoría existe y que los cambios son válidos.
        /// </remarks>
        Task<string> Editar(Categoria objeto);
    }
}
