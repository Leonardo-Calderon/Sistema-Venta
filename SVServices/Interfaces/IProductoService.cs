using SVRepository.Entities;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de productos del sistema de ventas.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la gestión de productos, proporcionando
    /// una capa de abstracción entre los controladores y los repositorios. Los productos son
    /// los elementos que se venden en el sistema e incluyen información sobre precios, stock y categoría.
    /// </remarks>
    public interface IProductoService
    {
        /// <summary>
        /// Obtiene una lista de productos opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar los productos por código o descripción. Si está vacío, retorna todos los productos.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de productos.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<List<Producto>> Lista(string buscar = "");

        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Producto con los datos del nuevo producto a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de delegar la operación
        /// al repositorio, como verificar duplicados o validar reglas de negocio.
        /// </remarks>
        Task<string> Crear(Producto objeto);

        /// <summary>
        /// Actualiza los datos de un producto existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Producto con los datos actualizados del producto.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de actualizar,
        /// como verificar que el producto existe y que los cambios son válidos.
        /// </remarks>
        Task<string> Editar(Producto objeto);

        /// <summary>
        /// Obtiene un producto específico por su código.
        /// </summary>
        /// <param name="codigo">Código único del producto a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el producto encontrado o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<Producto> Obtener(string codigo);
    }
}
