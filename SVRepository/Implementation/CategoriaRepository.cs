using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    /// <summary>
    /// Implementación del repositorio para la gestión de categorías de productos.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz ICategoriaRepository y proporciona métodos
    /// para acceder a los datos de categorías almacenados en la base de datos SQL Server.
    /// Las categorías se utilizan para organizar y clasificar los productos del sistema.
    /// </remarks>
    public class CategoriaRepository : ICategoriaRepository
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private readonly Conexion _conexion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase CategoriaRepository.
        /// </summary>
        /// <param name="conexion">Instancia de la clase Conexion para acceder a la base de datos.</param>
        /// <remarks>
        /// El constructor recibe una instancia de Conexion que será utilizada
        /// para establecer conexiones a la base de datos en todas las operaciones.
        /// </remarks>
        public CategoriaRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        /// <summary>
        /// Obtiene una lista de categorías opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar las categorías por nombre. Si está vacío, retorna todas las categorías.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de categorías.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_listaCategoria' que retorna todas las categorías activas
        /// o aquellas que coincidan con el término de búsqueda proporcionado. Incluye información de la medida asociada.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<Categoria>> Lista(string buscar = "")
        {
            List<Categoria> lista = new List<Categoria>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_listaCategoria", con);
                cmd.Parameters.Add(new SqlParameter("@Buscar", buscar ?? string.Empty));    
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Categoria
                        {
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                            Nombre = dr["Nombre"].ToString() ?? string.Empty,
                            Activo = Convert.ToInt32(dr["Activo"]),
                            RefMedida = new Medida
                            {
                                IdMedida = Convert.ToInt32(dr["IdMedida"]),
                                Nombre = dr["NombreMedida"].ToString() ?? string.Empty,
                            }
                        });
                    }
                }
            }
            
            return lista;
        }

        /// <summary>
        /// Crea una nueva categoría en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Categoria con los datos de la nueva categoría a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_crearCategoria' que valida los datos de la categoría
        /// y la inserta en la base de datos. Retorna un mensaje de confirmación si la operación es exitosa,
        /// o un mensaje de error en caso contrario.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el objeto categoria es null.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<string> Crear(Categoria objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            string respuesta = "";
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_crearCategoria", con);
                cmd.Parameters.Add(new SqlParameter("@Nombre", objeto.Nombre ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@IdMedida", objeto.RefMedida?.IdMedida ?? 0));
                cmd.Parameters.Add("@MsjError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@MsjError"].Value) ?? string.Empty;
                }
                catch (Exception ex)
                {
                    respuesta = $"Error al crear la categoría: {ex.Message}";
                }
            }
            
            return respuesta;
        }

        /// <summary>
        /// Actualiza los datos de una categoría existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Categoria con los datos actualizados de la categoría.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_editarCategoria' que actualiza los datos de una categoría existente,
        /// incluyendo su estado activo/inactivo. Retorna un mensaje de confirmación si la operación es exitosa,
        /// o un mensaje de error en caso contrario.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el objeto categoria es null.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<string> Editar(Categoria objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            string respuesta = "";
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_editarCategoria", con);
                cmd.Parameters.Add(new SqlParameter("@IdCategoria", objeto.IdCategoria));
                cmd.Parameters.Add(new SqlParameter("@Nombre", objeto.Nombre ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@IdMedida", objeto.RefMedida?.IdMedida ?? 0));
                cmd.Parameters.Add(new SqlParameter("@Activo", objeto.Activo));
                cmd.Parameters.Add("@MsjError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@MsjError"].Value) ?? string.Empty;
                }
                catch (Exception ex)
                {
                    respuesta = $"Error al editar la categoría: {ex.Message}";
                }
            }
            
            return respuesta;
        }
    }
}
