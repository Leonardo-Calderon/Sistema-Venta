using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    /// <summary>
    /// Implementación del repositorio para la gestión de menús por rol.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IMenuRolRepository y proporciona métodos
    /// para acceder a los datos de menús asociados a roles específicos en la base de datos SQL Server.
    /// Esta funcionalidad es esencial para el control de acceso basado en roles (RBAC).
    /// </remarks>
    public class MenuRolRepository : IMenuRolRepository
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private readonly Conexion _conexion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase MenuRolRepository.
        /// </summary>
        /// <param name="conexion">Instancia de la clase Conexion para acceder a la base de datos.</param>
        /// <remarks>
        /// El constructor recibe una instancia de Conexion que será utilizada
        /// para establecer conexiones a la base de datos en todas las operaciones.
        /// </remarks>
        public MenuRolRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        /// <summary>
        /// Obtiene una lista de menús disponibles para un rol específico.
        /// </summary>
        /// <param name="idRol">Identificador único del rol para el cual se obtienen los menús.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de menús disponibles para el rol.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_obtenerMenus' que retorna todos los menús
        /// habilitados para el rol especificado. La lista incluye información sobre menús padre
        /// y el estado activo de cada menú.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<MenuRol>> Lista(int idRol)
        {
            List<MenuRol> lista = new List<MenuRol>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_obtenerMenus", con);
                cmd.Parameters.Add(new SqlParameter("@IdRol", idRol));
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new MenuRol
                        {
                            IdMenu = Convert.ToInt32(dr["IdMenu"]),
                            NombreMenu = dr["NombreMenu"].ToString() ?? string.Empty,
                            IdMenuPadre = Convert.ToInt32(dr["IdMenuPadre"]),
                            Activo = Convert.ToBoolean(dr["Activo"]),
                        });
                    }
                }
            }
            
            return lista;
        }
    }
}
