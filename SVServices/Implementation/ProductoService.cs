using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de productos del sistema de ventas.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IProductoService y proporciona la lógica de negocio
    /// para la gestión de productos. Actúa como una capa de abstracción entre los controladores
    /// y los repositorios, permitiendo agregar validaciones y lógica de negocio adicional.
    /// Los productos son los elementos que se venden en el sistema e incluyen información
    /// sobre precios, stock y categoría.
    /// </remarks>
    public class ProductoService : IProductoService
    {
        /// <summary>
        /// Instancia del repositorio de productos para acceder a los datos.
        /// </summary>
        private readonly IProductoRepository _productoRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase ProductoService.
        /// </summary>
        /// <param name="productoRepository">Instancia del repositorio de productos.</param>
        /// <remarks>
        /// El constructor recibe una instancia del repositorio de productos que será utilizada
        /// para realizar las operaciones de base de datos.
        /// </remarks>
        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository ?? throw new ArgumentNullException(nameof(productoRepository));
        }

        /// <summary>
        /// Obtiene una lista de productos opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar los productos por código o descripción. Si está vacío, retorna todos los productos.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de productos.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<List<Producto>> Lista(string buscar = "")
        {
            return await _productoRepository.Lista(buscar ?? string.Empty);
        }

        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Producto con los datos del nuevo producto a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio como verificar duplicados o validar reglas específicas.
        /// </remarks>
        public async Task<string> Crear(Producto objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            return await _productoRepository.Crear(objeto);
        }

        /// <summary>
        /// Actualiza los datos de un producto existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Producto con los datos actualizados del producto.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio como verificar que el producto existe
        /// y que los cambios son válidos.
        /// </remarks>
        public async Task<string> Editar(Producto objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            return await _productoRepository.Editar(objeto);
        }

        /// <summary>
        /// Obtiene un producto específico por su código.
        /// </summary>
        /// <param name="codigo">Código único del producto a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el producto encontrado o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<Producto> Obtener(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código del producto no puede estar vacío.", nameof(codigo));

            return await _productoRepository.Obtener(codigo);
        }
    }
}
