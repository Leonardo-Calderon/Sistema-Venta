using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para la entidad Producto.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Repository para la gestión de productos del sistema de ventas,
    /// proporcionando métodos para listar, crear, editar y obtener productos específicos.
    /// Los productos incluyen información como código, descripción, precios, stock y categoría.
    /// </remarks>
    public interface IProductoRepository
    {
        /// <summary>
        /// Obtiene una lista de productos opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar los productos por código o descripción. Si está vacío, retorna todos los productos.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de productos.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna todos los productos activos
        /// o aquellos que coincidan con el término de búsqueda proporcionado.
        /// </remarks>
        Task<List<Producto>> Lista(string buscar = "");

        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Producto con los datos del nuevo producto a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método valida los datos del producto y lo inserta en la base de datos.
        /// Retorna un mensaje de confirmación si la operación es exitosa, o un mensaje de error en caso contrario.
        /// </remarks>
        Task<string> Crear(Producto objeto);

        /// <summary>
        /// Actualiza los datos de un producto existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Producto con los datos actualizados del producto.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método actualiza los datos de un producto existente, incluyendo su estado activo/inactivo.
        /// Retorna un mensaje de confirmación si la operación es exitosa, o un mensaje de error en caso contrario.
        /// </remarks>
        Task<string> Editar(Producto objeto);

        /// <summary>
        /// Obtiene un producto específico por su código.
        /// </summary>
        /// <param name="codigo">Código único del producto a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el producto encontrado o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que busca un producto específico por su código.
        /// Retorna el producto completo con su información de categoría y medida si existe.
        /// </remarks>
        Task<Producto> Obtener(string codigo);
    }
}
