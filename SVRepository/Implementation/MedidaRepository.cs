using SVRepository.Interfaces;
using SVRepository.Entities;
using SVRepository.DB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SVRepository.Implementation
{
    /// <summary>
    /// Implementación del repositorio para la gestión de unidades de medida.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IMedidaRepository y proporciona métodos
    /// para acceder a los datos de unidades de medida almacenados en la base de datos SQL Server.
    /// Las unidades de medida definen cómo se cuantifican los productos en el sistema.
    /// </remarks>
    public class MedidaRepository : IMedidaRepository
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private readonly Conexion _conexion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase MedidaRepository.
        /// </summary>
        /// <param name="conexion">Instancia de la clase Conexion para acceder a la base de datos.</param>
        /// <remarks>
        /// El constructor recibe una instancia de Conexion que será utilizada
        /// para establecer conexiones a la base de datos en todas las operaciones.
        /// </remarks>
        public MedidaRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        /// <summary>
        /// Obtiene una lista de todas las unidades de medida disponibles en el sistema.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de unidades de medida.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_listaMedida' que retorna todas las unidades
        /// de medida configuradas en el sistema. La lista incluye abreviatura, nombre, equivalente y valor.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<Medida>> Lista()
        {
            List<Medida> lista = new List<Medida>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_listaMedida", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                { 
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Medida
                        {
                            IdMedida = Convert.ToInt32(dr["IdMedida"]),
                            Nombre = dr["Nombre"].ToString() ?? string.Empty,
                            Abreviatura = dr["Abreviatura"].ToString() ?? string.Empty,
                            Equivalente = dr["Equivalente"].ToString() ?? string.Empty,
                            Valor = Convert.ToInt32(dr["Valor"])
                        });
                    }
                }
            }
            
            return lista;
        }
    }
}
