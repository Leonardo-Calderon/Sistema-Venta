using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    /// <summary>
    /// Implementación del repositorio para la gestión de usuarios del sistema.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IUsuarioRepository y proporciona métodos
    /// para acceder a los datos de usuarios almacenados en la base de datos SQL Server.
    /// Los usuarios son la base del sistema de autenticación y autorización, incluyendo
    /// funcionalidades para login, gestión de contraseñas y administración de usuarios.
    /// </remarks>
    public class UsuarioRepository : IUsuarioRepository
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private readonly Conexion _conexion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase UsuarioRepository.
        /// </summary>
        /// <param name="conexion">Instancia de la clase Conexion para acceder a la base de datos.</param>
        /// <remarks>
        /// El constructor recibe una instancia de Conexion que será utilizada
        /// para establecer conexiones a la base de datos en todas las operaciones.
        /// </remarks>
        public UsuarioRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        /// <summary>
        /// Obtiene una lista de usuarios opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar los usuarios por nombre completo o correo. Si está vacío, retorna todos los usuarios.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de usuarios.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_listaUsuario' que retorna todos los usuarios activos
        /// o aquellos que coincidan con el término de búsqueda proporcionado. Incluye información del rol asignado.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<Usuario>> Lista(string buscar = "")
        {
            List<Usuario> lista = new List<Usuario>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_listaUsuario", con);
                cmd.Parameters.Add(new SqlParameter("@Buscar", buscar ?? string.Empty));
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            RefRol = new Rol
                            {
                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                Nombre = dr["NombreRol"].ToString() ?? string.Empty,
                            },
                            NombreCompleto = dr["NombreCompleto"].ToString() ?? string.Empty,
                            Correo = dr["Correo"].ToString() ?? string.Empty,
                            NombreUsuario = dr["NombreUsuario"].ToString() ?? string.Empty,
                            Activo = Convert.ToInt32(dr["Activo"])
                        });
                    }
                }
            }
            
            return lista;
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Usuario con los datos del nuevo usuario a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_crearUsuario' que valida los datos del usuario
        /// y lo inserta en la base de datos. La contraseña se almacena de forma segura y se asigna un rol al usuario.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el objeto usuario es null.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<string> Crear(Usuario objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            string respuesta = "";
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_crearUsuario", con);
                cmd.Parameters.Add(new SqlParameter("@IdRol", objeto.RefRol?.IdRol ?? 0));
                cmd.Parameters.Add(new SqlParameter("@NombreCompleto", objeto.NombreCompleto ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@Correo", objeto.Correo ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@NombreUsuario", objeto.NombreUsuario ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@Clave", objeto.Clave ?? string.Empty));
                cmd.Parameters.Add("@MsjError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@MsjError"].Value) ?? string.Empty;
                }
                catch (Exception ex)
                {
                    respuesta = $"Error al crear el usuario: {ex.Message}";
                }
            }
            
            return respuesta;
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Usuario con los datos actualizados del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_editarUsuario' que actualiza los datos de un usuario existente,
        /// incluyendo su estado activo/inactivo. No permite cambiar la contraseña desde este método.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el objeto usuario es null.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<string> Editar(Usuario objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            string respuesta = "";
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_editarUsuario", con);
                cmd.Parameters.Add(new SqlParameter("@IdUsuario", objeto.IdUsuario));
                cmd.Parameters.Add(new SqlParameter("@IdRol", objeto.RefRol?.IdRol ?? 0));
                cmd.Parameters.Add(new SqlParameter("@NombreCompleto", objeto.NombreCompleto ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@Correo", objeto.Correo ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@NombreUsuario", objeto.NombreUsuario ?? string.Empty));
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
                    respuesta = $"Error al editar el usuario: {ex.Message}";
                }
            }
            
            return respuesta;
        }

        /// <summary>
        /// Elimina lógicamente un usuario del sistema.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_eliminarUsuario' que realiza una eliminación lógica del usuario,
        /// marcándolo como inactivo en lugar de eliminarlo físicamente de la base de datos.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<string> Eliminar(int idUsuario)
        {
            string respuesta = "";
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_eliminarUsuario", con);
                cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                cmd.Parameters.Add("@MsjError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@MsjError"].Value) ?? string.Empty;
                }
                catch (Exception ex)
                {
                    respuesta = $"Error al eliminar el usuario: {ex.Message}";
                }
            }
            
            return respuesta;
        }

        /// <summary>
        /// Autentica un usuario con su nombre de usuario y contraseña.
        /// </summary>
        /// <param name="usuario">Nombre de usuario para la autenticación.</param>
        /// <param name="clave">Contraseña del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el usuario autenticado o un objeto vacío si la autenticación falla.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_login' que valida las credenciales del usuario
        /// y retorna la información completa del usuario si la autenticación es exitosa,
        /// incluyendo su rol y permisos.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el usuario o clave son null o vacíos.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<Usuario> Login(string usuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentNullException(nameof(usuario));
            if (string.IsNullOrWhiteSpace(clave))
                throw new ArgumentNullException(nameof(clave));

            Usuario objeto = new Usuario();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_login", con);
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario);
                cmd.Parameters.AddWithValue("@Clave", clave);
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString() ?? string.Empty,
                            RefRol = new Rol
                            {
                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                Nombre = dr["NombreRol"].ToString() ?? string.Empty,
                            },
                            Correo = dr["Correo"].ToString() ?? string.Empty,
                            NombreUsuario = dr["NombreUsuario"].ToString() ?? string.Empty,
                            ResetearClave = Convert.ToInt32(dr["ResetearClave"]),
                            Activo = Convert.ToInt32(dr["Activo"])
                        };
                    }
                }
            }
            
            return objeto;
        }

        /// <summary>
        /// Verifica si existe un usuario con el correo electrónico especificado.
        /// </summary>
        /// <param name="correo">Correo electrónico a verificar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el ID del usuario si existe, o 0 si no existe.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_verificarCorreo' que busca un usuario
        /// con el correo electrónico especificado. Se utiliza principalmente para validar
        /// la unicidad del correo electrónico durante el proceso de registro o recuperación de contraseña.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el correo es null o vacío.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<int> VerificarCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                throw new ArgumentNullException(nameof(correo));

            int idUsuario;
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_verificarCorreo", con);
                cmd.Parameters.Add(new SqlParameter("@Correo", correo));
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    idUsuario = Convert.ToInt32(cmd.Parameters["@IdUsuario"].Value);
                }
                catch (Exception ex)
                {
                    idUsuario = 0;
                }
            }
            
            return idUsuario;
        }

        /// <summary>
        /// Obtiene un usuario específico por su identificador.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el usuario encontrado o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_obtenerUsuarioPorId' que busca un usuario específico por su ID.
        /// Retorna el usuario completo con su información de rol si existe.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<Usuario> ObtenerPorId(int idUsuario)
        {
            Usuario objeto = new Usuario();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_obtenerUsuarioPorId", con);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString() ?? string.Empty,
                            RefRol = new Rol
                            {
                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                Nombre = dr["NombreRol"].ToString() ?? string.Empty,
                            },
                            Correo = dr["Correo"].ToString() ?? string.Empty,
                            NombreUsuario = dr["NombreUsuario"].ToString() ?? string.Empty,
                            Activo = Convert.ToInt32(dr["Activo"])
                        };
                    }
                }
            }
            
            return objeto;
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario y opcionalmente resetea el flag de cambio de contraseña.
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario.</param>
        /// <param name="nuevaClave">Nueva contraseña del usuario.</param>
        /// <param name="resetear">Flag que indica si se debe resetear el flag de cambio de contraseña (1 = sí, 0 = no).</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_actualizarClave' que actualiza la contraseña del usuario
        /// y opcionalmente resetea el flag que indica si el usuario debe cambiar su contraseña
        /// en el próximo inicio de sesión.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando la nueva clave es null o vacía.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task ActualizarClave(int idUsuario, string nuevaClave, int resetear)
        {
            if (string.IsNullOrWhiteSpace(nuevaClave))
                throw new ArgumentNullException(nameof(nuevaClave));

            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_actualizarClave", con);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@NuevaClave", nuevaClave);
                cmd.Parameters.AddWithValue("@Resetear", resetear);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al actualizar la contraseña: {ex.Message}", ex);
                }
            }
        }
    }
}
