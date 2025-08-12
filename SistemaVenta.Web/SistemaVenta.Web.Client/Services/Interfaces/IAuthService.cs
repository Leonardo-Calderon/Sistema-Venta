// En: SistemaVenta.Web.Client/Services/Contracts/IAuthService.cs
using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de autenticación en la aplicación cliente.
    /// </summary>
    /// <remarks>
    /// Esta interfaz define los métodos necesarios para manejar la autenticación
    /// de usuarios en la aplicación Blazor WebAssembly. Proporciona funcionalidad
    /// para iniciar y cerrar sesión de usuarios.
    /// 
    /// La implementación de esta interfaz se encarga de:
    /// - Comunicación con la API de autenticación
    /// - Almacenamiento local de tokens JWT
    /// - Gestión del estado de autenticación
    /// - Manejo de errores de autenticación
    /// </remarks>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica un usuario con las credenciales proporcionadas.
        /// </summary>
        /// <param name="loginDto">Datos de login del usuario (nombre de usuario y contraseña).</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. El resultado contiene
        /// los datos de sesión del usuario si la autenticación es exitosa.
        /// </returns>
        /// <remarks>
        /// Este método:
        /// 1. Envía las credenciales a la API de autenticación
        /// 2. Recibe un token JWT si las credenciales son válidas
        /// 3. Almacena el token en el almacenamiento local
        /// 4. Retorna los datos de sesión del usuario
        /// 
        /// Si la autenticación falla, lanza una excepción con el mensaje de error.
        /// </remarks>
        Task<SessionDTO> Login(LoginDTO loginDto);

        /// <summary>
        /// Cierra la sesión del usuario actual.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método:
        /// 1. Elimina el token JWT del almacenamiento local
        /// 2. Limpia cualquier estado de sesión
        /// 3. Notifica al sistema de autenticación sobre el cierre de sesión
        /// 
        /// Después de llamar a este método, el usuario será redirigido a la página de login.
        /// </remarks>
        Task Logout();
    }
}