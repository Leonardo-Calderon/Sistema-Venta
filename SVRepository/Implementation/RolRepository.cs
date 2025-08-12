
using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    /// <summary>
    /// Implementación del repositorio para la gestión de roles de usuario.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IRolRepository y proporciona métodos
    /// para acceder a los datos de roles almacenados en la base de datos SQL Server.
    /// Utiliza stored procedures para realizar las operaciones de base de datos.
    /// </remarks>
    public class RolRepository : IRolRepository
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private readonly Conexion _conexion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase RolRepository.
        /// </summary>
        /// <param name="conexion">Instancia de la clase Conexion para acceder a la base de datos.</param>
        /// <remarks>
        /// El constructor recibe una instancia de Conexion que será utilizada
        /// para establecer conexiones a la base de datos en todas las operaciones.
        /// </remarks>
        public RolRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        /// <summary>
        /// Obtiene una lista de todos los roles disponibles en el sistema.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de roles.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_listaRol' que retorna todos los roles
        /// configurados en el sistema. La lista incluye el identificador y nombre de cada rol.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<Rol>> Lista()
        {
            List<Rol> lista = new List<Rol>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_listaRol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Rol
                        {
                            IdRol = Convert.ToInt32(dr["IdRol"]),
                            Nombre = dr["Nombre"].ToString() ?? string.Empty,
                        });
                    }
                }
            }
            
            return lista;
        }
    }
}
