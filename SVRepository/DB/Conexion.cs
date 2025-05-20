
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SVRepository.DB
{
    public class Conexion
    {
        private readonly IConfiguration _configuracion;
        private readonly string _cadenaSql;

        public Conexion(IConfiguration configuracion){
            _configuracion = configuracion;
            _cadenaSql = _configuracion.GetConnectionString("cadenaSQL")!;
        }
        public SqlConnection ObtenerSQLConexion()
        {
            return new SqlConnection(_cadenaSql);
        }

    }
}
