using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para la entidad Usuario.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Repository para la gestión de usuarios del sistema,
    /// proporcionando métodos para autenticación, gestión de usuarios y recuperación de contraseñas.
    /// Los usuarios son la base del sistema de autenticación y autorización.
    /// </remarks>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Obtiene una lista de usuarios opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar los usuarios por nombre completo o correo. Si está vacío, retorna todos los usuarios.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de usuarios.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna todos los usuarios activos
        /// o aquellos que coincidan con el término de búsqueda proporcionado.
        /// </remarks>
        Task<List<Usuario>> Lista(string buscar = "");

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Usuario con los datos del nuevo usuario a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método valida los datos del usuario y lo inserta en la base de datos.
        /// La contraseña se almacena de forma segura y se asigna un rol al usuario.
        /// </remarks>
        Task<string> Crear(Usuario objeto);

        /// <summary>
        /// Actualiza los datos de un usuario existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Usuario con los datos actualizados del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método actualiza los datos de un usuario existente, incluyendo su estado activo/inactivo.
        /// No permite cambiar la contraseña desde este método.
        /// </remarks>
        Task<string> Editar(Usuario objeto);

        /// <summary>
        /// Elimina lógicamente un usuario del sistema.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método realiza una eliminación lógica del usuario, marcándolo como inactivo
        /// en lugar de eliminarlo físicamente de la base de datos.
        /// </remarks>
        Task<string> Eliminar(int idUsuario);

        /// <summary>
        /// Autentica un usuario con su nombre de usuario y contraseña.
        /// </summary>
        /// <param name="usuario">Nombre de usuario para la autenticación.</param>
        /// <param name="clave">Contraseña del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el usuario autenticado o un objeto vacío si la autenticación falla.</returns>
        /// <remarks>
        /// Este método valida las credenciales del usuario y retorna la información completa
        /// del usuario si la autenticación es exitosa, incluyendo su rol y permisos.
        /// </remarks>
        Task<Usuario> Login(string usuario, string clave);

        /// <summary>
        /// Obtiene un usuario específico por su identificador.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el usuario encontrado o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que busca un usuario específico por su ID.
        /// Retorna el usuario completo con su información de rol si existe.
        /// </remarks>
        Task<Usuario> ObtenerPorId(int idUsuario);

        /// <summary>
        /// Verifica si existe un usuario con el correo electrónico especificado.
        /// </summary>
        /// <param name="correo">Correo electrónico a verificar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el ID del usuario si existe, o 0 si no existe.</returns>
        /// <remarks>
        /// Este método se utiliza principalmente para validar la unicidad del correo electrónico
        /// durante el proceso de registro o recuperación de contraseña.
        /// </remarks>
        Task<int> VerificarCorreo(string correo);

        /// <summary>
        /// Actualiza la contraseña de un usuario y opcionalmente resetea el flag de cambio de contraseña.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario.</param>
        /// <param name="nuevaClave">Nueva contraseña del usuario.</param>
        /// <param name="resetear">Flag que indica si se debe resetear el flag de cambio de contraseña (1 = sí, 0 = no).</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método actualiza la contraseña del usuario y opcionalmente resetea el flag
        /// que indica si el usuario debe cambiar su contraseña en el próximo inicio de sesión.
        /// </remarks>
        Task ActualizarClave(int idUsuario, string nuevaClave, int resetear);
    }
}
