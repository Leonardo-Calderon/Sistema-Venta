using System.Security.Claims;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la auditoría de seguridad del sistema.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la auditoría de seguridad, proporcionando
    /// métodos para registrar y consultar eventos de seguridad como accesos, autenticaciones
    /// y autorizaciones. La auditoría es esencial para el cumplimiento de políticas de seguridad
    /// y la detección de actividades sospechosas.
    /// </remarks>
    public interface IAuditoriaService
    {
        /// <summary>
        /// Registra un intento de acceso a un endpoint del sistema.
        /// </summary>
        /// <param name="usuario">Información del usuario que realiza el acceso.</param>
        /// <param name="endpoint">Endpoint o ruta accedida en el sistema.</param>
        /// <param name="metodo">Método HTTP utilizado (GET, POST, PUT, DELETE, etc.).</param>
        /// <param name="resultado">Resultado del acceso (Permitido/Denegado).</param>
        /// <param name="detalles">Detalles adicionales del acceso como parámetros, headers, etc.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método registra cada intento de acceso a los endpoints del sistema,
        /// proporcionando información detallada para análisis de seguridad y cumplimiento.
        /// </remarks>
        Task RegistrarAcceso(ClaimsPrincipal usuario, string endpoint, string metodo, string resultado, string detalles = "");

        /// <summary>
        /// Registra un intento de autorización a un recurso específico.
        /// </summary>
        /// <param name="usuario">Información del usuario que intenta la autorización.</param>
        /// <param name="recurso">Recurso al que se intenta acceder (archivo, función, dato, etc.).</param>
        /// <param name="accion">Acción que se intenta realizar (leer, escribir, eliminar, etc.).</param>
        /// <param name="resultado">Resultado de la autorización (Permitido/Denegado).</param>
        /// <param name="motivo">Motivo del resultado de la autorización.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método registra los intentos de autorización para recursos específicos,
        /// permitiendo el seguimiento de permisos y la detección de accesos no autorizados.
        /// </remarks>
        Task RegistrarAutorizacion(ClaimsPrincipal usuario, string recurso, string accion, string resultado, string motivo = "");

        /// <summary>
        /// Registra un intento de autenticación en el sistema.
        /// </summary>
        /// <param name="nombreUsuario">Nombre de usuario que intenta autenticarse.</param>
        /// <param name="resultado">Resultado del intento de login (Exitoso/Fallido).</param>
        /// <param name="ipAddress">Dirección IP del cliente que realiza el intento.</param>
        /// <param name="detalles">Detalles adicionales como navegador, sistema operativo, etc.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método registra todos los intentos de autenticación, exitosos y fallidos,
        /// para detectar patrones de ataque como fuerza bruta o accesos desde ubicaciones sospechosas.
        /// </remarks>
        Task RegistrarAutenticacion(string nombreUsuario, string resultado, string ipAddress, string detalles = "");

        /// <summary>
        /// Obtiene el historial de auditoría para un usuario específico en un rango de fechas.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario.</param>
        /// <param name="fechaInicio">Fecha de inicio del período de consulta.</param>
        /// <param name="fechaFin">Fecha de fin del período de consulta.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de registros de auditoría.</returns>
        /// <remarks>
        /// Este método permite consultar el historial completo de actividades de un usuario
        /// específico, útil para investigaciones de seguridad y cumplimiento de políticas.
        /// </remarks>
        Task<List<object>> ObtenerHistorialAuditoria(int idUsuario, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene estadísticas de auditoría para administradores en un rango de fechas.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del período de análisis.</param>
        /// <param name="fechaFin">Fecha de fin del período de análisis.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado son estadísticas de auditoría.</returns>
        /// <remarks>
        /// Este método genera estadísticas agregadas de auditoría para administradores,
        /// incluyendo métricas como intentos de acceso fallidos, accesos no autorizados,
        /// patrones de uso, etc.
        /// </remarks>
        Task<object> ObtenerEstadisticasAuditoria(DateTime fechaInicio, DateTime fechaFin);
    }
} 