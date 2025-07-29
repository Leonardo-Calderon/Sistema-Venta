using System.Security.Claims;

namespace SVServices.Interfaces
{
    public interface IAuditoriaService
    {
        /// <summary>
        /// Registra un intento de acceso a un endpoint
        /// </summary>
        /// <param name="usuario">Información del usuario</param>
        /// <param name="endpoint">Endpoint accedido</param>
        /// <param name="metodo">Método HTTP</param>
        /// <param name="resultado">Resultado del acceso (Permitido/Denegado)</param>
        /// <param name="detalles">Detalles adicionales del acceso</param>
        Task RegistrarAcceso(ClaimsPrincipal usuario, string endpoint, string metodo, string resultado, string detalles = "");

        /// <summary>
        /// Registra un intento de autorización
        /// </summary>
        /// <param name="usuario">Información del usuario</param>
        /// <param name="recurso">Recurso al que se intenta acceder</param>
        /// <param name="accion">Acción realizada</param>
        /// <param name="resultado">Resultado de la autorización</param>
        /// <param name="motivo">Motivo del resultado</param>
        Task RegistrarAutorizacion(ClaimsPrincipal usuario, string recurso, string accion, string resultado, string motivo = "");

        /// <summary>
        /// Registra un intento de autenticación
        /// </summary>
        /// <param name="nombreUsuario">Nombre de usuario</param>
        /// <param name="resultado">Resultado del login</param>
        /// <param name="ipAddress">Dirección IP del cliente</param>
        /// <param name="detalles">Detalles adicionales</param>
        Task RegistrarAutenticacion(string nombreUsuario, string resultado, string ipAddress, string detalles = "");

        /// <summary>
        /// Obtiene el historial de auditoría para un usuario específico
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Lista de registros de auditoría</returns>
        Task<List<object>> ObtenerHistorialAuditoria(int idUsuario, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene estadísticas de auditoría para administradores
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Estadísticas de auditoría</returns>
        Task<object> ObtenerEstadisticasAuditoria(DateTime fechaInicio, DateTime fechaFin);
    }
} 