// En: SVRepository/DB/Conexion.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace SVRepository.DB
{
    public class Conexion
    {
        private string connectionString = string.Empty;

        // Cambiamos el constructor para que reciba IConfiguration
        public Conexion(IConfiguration configuration)
        {
            // La clase extrae la cadena de conexión que necesita
            connectionString = configuration.GetConnectionString("cadenaSQL")!;
        }

        public SqlConnection ObtenerSQLConexion()
        {
            return new SqlConnection(connectionString);
        }
    }
}