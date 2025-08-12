using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para la entidad Categoria.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Repository para la gestión de categorías de productos,
    /// proporcionando métodos para listar, crear y editar categorías en el sistema de ventas.
    /// </remarks>
    public interface ICategoriaRepository
    {
        /// <summary>
        /// Obtiene una lista de categorías opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar las categorías por nombre. Si está vacío, retorna todas las categorías.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de categorías.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna todas las categorías activas
        /// o aquellas que coincidan con el término de búsqueda proporcionado.
        /// </remarks>
        Task<List<Categoria>> Lista(string buscar = "");

        /// <summary>
        /// Crea una nueva categoría en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Categoria con los datos de la nueva categoría a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método valida los datos de la categoría y la inserta en la base de datos.
        /// Retorna un mensaje de confirmación si la operación es exitosa, o un mensaje de error en caso contrario.
        /// </remarks>
        Task<string> Crear(Categoria objeto);

        /// <summary>
        /// Actualiza los datos de una categoría existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Categoria con los datos actualizados de la categoría.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método actualiza los datos de una categoría existente, incluyendo su estado activo/inactivo.
        /// Retorna un mensaje de confirmación si la operación es exitosa, o un mensaje de error en caso contrario.
        /// </remarks>
        Task<string> Editar(Categoria objeto);
    }
}
