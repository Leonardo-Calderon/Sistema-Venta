using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly Conexion _conexion;
        public UsuarioRepository(Conexion conexion)
        {
            _conexion = conexion;
        }

        public async Task<List<Usuario>> Lista(string buscar = "")
        {
            List<Usuario> lista = new List<Usuario>();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_listaUsuario", con);
                cmd.Parameters.Add(new SqlParameter("@Buscar", buscar));
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
                                Nombre = dr["NombreRol"].ToString(),
                            },
                            NombreCompleto = dr["NombreCompleto"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            NombreUsuario = dr["NombreUsuario"].ToString(),
                            Activo = Convert.ToInt32(dr["Activo"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<string> Crear(Usuario objeto)
        {
            string respuesta = "";
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_crearUsuario", con);
                cmd.Parameters.Add(new SqlParameter("@IdRol", objeto.RefRol.IdRol));
                cmd.Parameters.Add(new SqlParameter("@NombreCompleto", objeto.NombreCompleto));
                cmd.Parameters.Add(new SqlParameter("@Correo", objeto.Correo));
                cmd.Parameters.Add(new SqlParameter("@NombreUsuario", objeto.NombreUsuario));
                cmd.Parameters.Add(new SqlParameter("@Clave", objeto.Clave));   
                cmd.Parameters.Add("@MsjError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@MsjError"].Value)!;
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }

            }
            return respuesta;
        }

        public async Task<string> Editar(Usuario objeto)
        {
            string respuesta = "";
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_editarUsuario", con);
                cmd.Parameters.Add(new SqlParameter("@IdUsuario", objeto.IdUsuario));
                cmd.Parameters.Add(new SqlParameter("@IdRol", objeto.RefRol.IdRol));
                cmd.Parameters.Add(new SqlParameter("@NombreCompleto", objeto.NombreCompleto));
                cmd.Parameters.Add(new SqlParameter("@Correo", objeto.Correo));
                cmd.Parameters.Add(new SqlParameter("@NombreUsuario", objeto.NombreUsuario));
                cmd.Parameters.Add(new SqlParameter("@Activo", objeto.Activo));
                cmd.Parameters.Add("@MsjError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@MsjError"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }

            }
            return respuesta;
        }

        public async Task<string> Eliminar(int idUsuario)
        {
            string respuesta = "";
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_eliminarUsuario", con);
                cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                cmd.Parameters.Add("@MsjError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@MsjError"].Value)!;
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }
            }
            return respuesta;
        }

        public async Task<Usuario> Login(string usuario, string clave)
        {
            Usuario objeto = new Usuario();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
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
                            NombreCompleto = dr["NombreCompleto"].ToString(),
                            RefRol = new Rol
                            {
                                IdRol = Convert.ToInt32(dr["IdRol"]),
                                Nombre = dr["NombreRol"].ToString(),
                            },
                            Correo = dr["Correo"].ToString(),
                            NombreUsuario = dr["NombreUsuario"].ToString(),
                            ResetearClave = Convert.ToInt32(dr["ResetearClave"]),
                            Activo = Convert.ToInt32(dr["Activo"])
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<int> VerificarCorreo(string correo)
        {
            int idUsuario;
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
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

        public async Task ActualizarClave(int idUsuario, string nuevaClave, int resetear)
        {
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
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
                    throw new Exception(ex.Message);
                }

            }

        }
    }
}
