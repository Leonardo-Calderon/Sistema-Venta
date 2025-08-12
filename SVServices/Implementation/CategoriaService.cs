using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de categorías de productos.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz ICategoriaService y proporciona la lógica de negocio
    /// para la gestión de categorías. Actúa como una capa de abstracción entre los controladores
    /// y los repositorios, permitiendo agregar validaciones y lógica de negocio adicional.
    /// </remarks>
    public class CategoriaService : ICategoriaService
    {
        /// <summary>
        /// Instancia del repositorio de categorías para acceder a los datos.
        /// </summary>
        private readonly ICategoriaRepository _categoriaRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase CategoriaService.
        /// </summary>
        /// <param name="categoriaRepository">Instancia del repositorio de categorías.</param>
        /// <remarks>
        /// El constructor recibe una instancia del repositorio de categorías que será utilizada
        /// para realizar las operaciones de base de datos.
        /// </remarks>
        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
        }

        /// <summary>
        /// Obtiene una lista de categorías opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar las categorías por nombre. Si está vacío, retorna todas las categorías.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de categorías.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<List<Categoria>> Lista(string buscar = "")
        {
            return await _categoriaRepository.Lista(buscar ?? string.Empty);
        }

        /// <summary>
        /// Crea una nueva categoría en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Categoria con los datos de la nueva categoría a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio como verificar duplicados o validar reglas específicas.
        /// </remarks>
        public async Task<string> Crear(Categoria objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            return await _categoriaRepository.Crear(objeto);
        }

        /// <summary>
        /// Actualiza los datos de una categoría existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Categoria con los datos actualizados de la categoría.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio como verificar que la categoría existe
        /// y que los cambios son válidos.
        /// </remarks>
        public async Task<string> Editar(Categoria objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            return await _categoriaRepository.Editar(objeto);
        }
    }
}
