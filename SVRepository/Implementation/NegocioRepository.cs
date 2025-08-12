using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    /// <summary>
    /// Implementación del repositorio para la gestión de información del negocio.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz INegocioRepository y proporciona métodos
    /// para acceder y actualizar los datos de configuración del negocio en la base de datos SQL Server.
    /// La información del negocio incluye datos fiscales, de contacto y configuración del sistema.
    /// </remarks>
    public class NegocioRepository : INegocioRepository
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private readonly Conexion _conexion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase NegocioRepository.
        /// </summary>
        /// <param name="conexion">Instancia de la clase Conexion para acceder a la base de datos.</param>
        /// <remarks>
        /// El constructor recibe una instancia de Conexion que será utilizada
        /// para establecer conexiones a la base de datos en todas las operaciones.
        /// </remarks>
        public NegocioRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        /// <summary>
        /// Obtiene la información de configuración del negocio.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un objeto Negocio con la información del negocio.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_obtenerNegocio' que retorna la información completa
        /// del negocio, incluyendo datos de contacto, dirección, configuración de moneda y logo.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<Negocio> Obtener()
        {
            Negocio objeto = new Negocio();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_obtenerNegocio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Negocio
                        {
                            RazonSocial = dr["RazonSocial"].ToString() ?? string.Empty,
                            RFC = dr["RFC"].ToString() ?? string.Empty,
                            Direccion = dr["Direccion"].ToString() ?? string.Empty,
                            Celular = dr["Celular"].ToString() ?? string.Empty,
                            Correo = dr["Correo"].ToString() ?? string.Empty,
                            SimboloMoneda = dr["SimboloMoneda"].ToString() ?? string.Empty,
                            NombreLogo = dr["NombreLogo"].ToString() ?? string.Empty,
                            URL = dr["URL"].ToString() ?? string.Empty
                        };
                    }
                }
            }
            
            return objeto;
        }

        /// <summary>
        /// Actualiza la información de configuración del negocio.
        /// </summary>
        /// <param name="objeto">Objeto Negocio con los datos actualizados del negocio.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_editarNegocio' que actualiza todos los datos
        /// de configuración del negocio en la base de datos. Si ocurre un error durante la actualización,
        /// se lanza una excepción con el mensaje de error.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task Editar(Negocio objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_editarNegocio", con);
                cmd.Parameters.Add(new SqlParameter("@RazonSocial", objeto.RazonSocial ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@RFC", objeto.RFC ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@Direccion", objeto.Direccion ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@Celular", objeto.Celular ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@Correo", objeto.Correo ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@SimboloMoneda", objeto.SimboloMoneda ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@NombreLogo", objeto.NombreLogo ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@URL", objeto.URL ?? string.Empty));
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al actualizar la información del negocio: {ex.Message}", ex);
                }
            }
        }
    }
}
