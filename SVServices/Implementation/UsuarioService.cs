using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de usuarios del sistema.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IUsuarioService y proporciona la lógica de negocio
    /// para la gestión de usuarios. Actúa como una capa de abstracción entre los controladores
    /// y los repositorios, permitiendo agregar validaciones y lógica de negocio adicional.
    /// Los usuarios son la base del sistema de autenticación y autorización, incluyendo
    /// funcionalidades para login, gestión de contraseñas y administración de usuarios.
    /// </remarks>
    public class UsuarioService : IUsuarioService
    {
        /// <summary>
        /// Instancia del repositorio de usuarios para acceder a los datos.
        /// </summary>
        private readonly IUsuarioRepository _usuarioRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase UsuarioService.
        /// </summary>
        /// <param name="usuarioRepository">Instancia del repositorio de usuarios.</param>
        /// <remarks>
        /// El constructor recibe una instancia del repositorio de usuarios que será utilizada
        /// para realizar las operaciones de base de datos.
        /// </remarks>
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }

        /// <summary>
        /// Obtiene una lista de usuarios opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar los usuarios por nombre completo o correo. Si está vacío, retorna todos los usuarios.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de usuarios.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<List<Usuario>> Lista(string buscar = "")
        {
            return await _usuarioRepository.Lista(buscar ?? string.Empty);
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Usuario con los datos del nuevo usuario a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio como verificar duplicados o validar reglas específicas.
        /// </remarks>
        public async Task<string> Crear(Usuario objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            return await _usuarioRepository.Crear(objeto);
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Usuario con los datos actualizados del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio como verificar que el usuario existe
        /// y que los cambios son válidos.
        /// </remarks>
        public async Task<string> Editar(Usuario objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            return await _usuarioRepository.Editar(objeto);
        }

        /// <summary>
        /// Elimina lógicamente un usuario del sistema.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio como verificar que el usuario no tiene
        /// operaciones pendientes.
        /// </remarks>
        public async Task<string> Eliminar(int idUsuario)
        {
            return await _usuarioRepository.Eliminar(idUsuario);
        }

        /// <summary>
        /// Autentica un usuario con su nombre de usuario y contraseña.
        /// </summary>
        /// <param name="usuario">Nombre de usuario para la autenticación.</param>
        /// <param name="clave">Contraseña del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el usuario autenticado o un objeto vacío si la autenticación falla.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar lógica adicional de seguridad como logging de intentos
        /// de acceso, bloqueo temporal de cuentas, etc.
        /// </remarks>
        public async Task<Usuario> Login(string usuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(usuario));

            if (string.IsNullOrWhiteSpace(clave))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(clave));

            return await _usuarioRepository.Login(usuario, clave);
        }

        /// <summary>
        /// Obtiene un usuario específico por su identificador.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el usuario encontrado o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<Usuario> ObtenerPorId(int idUsuario)
        {
            return await _usuarioRepository.ObtenerPorId(idUsuario);
        }

        /// <summary>
        /// Verifica si existe un usuario con el correo electrónico especificado.
        /// </summary>
        /// <param name="correo">Correo electrónico a verificar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el ID del usuario si existe, o 0 si no existe.</returns>
        /// <remarks>
        /// Este método se utiliza principalmente para validar la unicidad del correo electrónico
        /// durante el proceso de registro o recuperación de contraseña.
        /// </remarks>
        public async Task<int> VerificarCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                throw new ArgumentException("El correo electrónico no puede estar vacío.", nameof(correo));

            return await _usuarioRepository.VerificarCorreo(correo);
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario y opcionalmente resetea el flag de cambio de contraseña.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario.</param>
        /// <param name="nuevaClave">Nueva contraseña del usuario.</param>
        /// <param name="resetear">Flag que indica si se debe resetear el flag de cambio de contraseña (1 = sí, 0 = no).</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de seguridad como verificar la complejidad
        /// de la contraseña o el historial de contraseñas utilizadas.
        /// </remarks>
        public async Task ActualizarClave(int idUsuario, string nuevaClave, int resetear)
        {
            if (string.IsNullOrWhiteSpace(nuevaClave))
                throw new ArgumentException("La nueva contraseña no puede estar vacía.", nameof(nuevaClave));

            await _usuarioRepository.ActualizarClave(idUsuario, nuevaClave, resetear);
        }
    }
}
