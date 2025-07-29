using System.Data;

namespace SVServices.Interfaces
{
    public interface ISeguridadSQLService
    {
        /// <summary>
        /// Analiza cómo se ejecuta una consulta parametrizada y explica por qué es segura
        /// </summary>
        /// <param name="nombreProcedimiento">Nombre del stored procedure</param>
        /// <param name="parametros">Parámetros de la consulta</param>
        /// <returns>Análisis de seguridad de la consulta</returns>
        Task<string> AnalizarConsultaParametrizada(string nombreProcedimiento, Dictionary<string, object> parametros);

        /// <summary>
        /// Simula una consulta vulnerable para demostrar la diferencia
        /// </summary>
        /// <param name="consultaVulnerable">Consulta SQL vulnerable</param>
        /// <returns>Ejemplo de cómo sería vulnerable</returns>
        string SimularConsultaVulnerable(string consultaVulnerable);

        /// <summary>
        /// Genera un reporte de seguridad para un stored procedure
        /// </summary>
        /// <param name="nombreProcedimiento">Nombre del SP</param>
        /// <param name="parametros">Parámetros del SP</param>
        /// <returns>Reporte detallado de seguridad</returns>
        Task<object> GenerarReporteSeguridad(string nombreProcedimiento, Dictionary<string, object> parametros);

        /// <summary>
        /// Verifica si una consulta está usando parámetros correctamente
        /// </summary>
        /// <param name="comando">Comando SQL a verificar</param>
        /// <returns>True si es seguro, false si es vulnerable</returns>
        bool VerificarUsoParametros(IDbCommand comando);

        /// <summary>
        /// Documenta el flujo de seguridad de una operación
        /// </summary>
        /// <param name="operacion">Tipo de operación (INSERT, UPDATE, DELETE, SELECT)</param>
        /// <param name="entidad">Nombre de la entidad</param>
        /// <returns>Documentación del flujo de seguridad</returns>
        string DocumentarFlujoSeguridad(string operacion, string entidad);

        /// <summary>
        /// Verifica si una entrada contiene caracteres peligrosos para SQL Injection
        /// </summary>
        /// <param name="entrada">Entrada a verificar</param>
        /// <returns>True si contiene caracteres peligrosos</returns>
        bool ContieneCaracteresPeligrosos(string entrada);

        /// <summary>
        /// Sanitiza una entrada contra SQL Injection
        /// </summary>
        /// <param name="entrada">Entrada a sanitizar</param>
        /// <returns>Entrada sanitizada</returns>
        string SanitizarContraSQLInjection(string entrada);
    }
} 