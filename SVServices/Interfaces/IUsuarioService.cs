

using SVRepository.Entities;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de usuarios del sistema.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la gestión de usuarios, proporcionando
    /// una capa de abstracción entre los controladores y los repositorios. Los usuarios son
    /// la base del sistema de autenticación y autorización, incluyendo funcionalidades para
    /// login, gestión de contraseñas y administración de usuarios.
    /// </remarks>
    public interface IUsuarioService
    {
        /// <summary>
        /// Obtiene una lista de usuarios opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar los usuarios por nombre completo o correo. Si está vacío, retorna todos los usuarios.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de usuarios.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<List<Usuario>> Lista(string buscar = "");

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Usuario con los datos del nuevo usuario a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de delegar la operación
        /// al repositorio, como verificar duplicados o validar reglas de negocio.
        /// </remarks>
        Task<string> Crear(Usuario objeto);

        /// <summary>
        /// Actualiza los datos de un usuario existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Usuario con los datos actualizados del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de actualizar,
        /// como verificar que el usuario existe y que los cambios son válidos.
        /// </remarks>
        Task<string> Editar(Usuario objeto);

        /// <summary>
        /// Elimina lógicamente un usuario del sistema.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de eliminar,
        /// como verificar que el usuario no tiene operaciones pendientes.
        /// </remarks>
        Task<string> Eliminar(int idUsuario);

        /// <summary>
        /// Autentica un usuario con su nombre de usuario y contraseña.
        /// </summary>
        /// <param name="usuario">Nombre de usuario para la autenticación.</param>
        /// <param name="clave">Contraseña del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el usuario autenticado o un objeto vacío si la autenticación falla.</returns>
        /// <remarks>
        /// Este método puede incluir lógica adicional de seguridad como logging de intentos
        /// de acceso, bloqueo temporal de cuentas, etc.
        /// </remarks>
        Task<Usuario> Login(string usuario, string clave);

        /// <summary>
        /// Obtiene un usuario específico por su identificador.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el usuario encontrado o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
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
        /// Este método puede incluir validaciones de seguridad como verificar la complejidad
        /// de la contraseña o el historial de contraseñas utilizadas.
        /// </remarks>
        Task ActualizarClave(int idUsuario, string nuevaClave, int resetear);
    }
}
