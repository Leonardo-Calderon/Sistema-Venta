// En: SVRepository/DB/Conexion.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace SVRepository.DB
{
    /// <summary>
    /// Clase responsable de gestionar la conexión a la base de datos SQL Server.
    /// </summary>
    /// <remarks>
    /// Esta clase encapsula la lógica de conexión a la base de datos, proporcionando
    /// una interfaz unificada para obtener conexiones SQL Server configuradas.
    /// Utiliza el patrón de inyección de dependencias para recibir la configuración.
    /// </remarks>
    public class Conexion
    {
        /// <summary>
        /// Cadena de conexión a la base de datos SQL Server.
        /// </summary>
        private readonly string connectionString = string.Empty;

        /// <summary>
        /// Inicializa una nueva instancia de la clase Conexion con la configuración proporcionada.
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación que contiene la cadena de conexión.</param>
        /// <remarks>
        /// El constructor extrae la cadena de conexión del archivo de configuración
        /// utilizando la clave "cadenaSQL". Esta cadena debe estar definida en el
        /// archivo appsettings.json o en la configuración de la aplicación.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando configuration es null.</exception>
        public Conexion(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            connectionString = configuration.GetConnectionString("cadenaSQL") 
                ?? throw new InvalidOperationException("La cadena de conexión 'cadenaSQL' no está configurada.");
        }

        /// <summary>
        /// Crea y retorna una nueva conexión SQL Server configurada.
        /// </summary>
        /// <returns>Una nueva instancia de SqlConnection configurada con la cadena de conexión.</returns>
        /// <remarks>
        /// Este método crea una nueva conexión SQL Server pero no la abre.
        /// Es responsabilidad del código cliente abrir, usar y cerrar la conexión.
        /// Se recomienda usar la conexión dentro de un bloque using para garantizar
        /// la liberación adecuada de recursos.
        /// </remarks>
        /// <example>
        /// <code>
        /// using (var connection = conexion.ObtenerSQLConexion())
        /// {
        ///     await connection.OpenAsync();
        ///     // Usar la conexión
        /// }
        /// </code>
        /// </example>
        public SqlConnection ObtenerSQLConexion()
        {
            return new SqlConnection(connectionString);
        }
    }
}