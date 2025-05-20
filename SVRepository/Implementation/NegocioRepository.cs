using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    public class NegocioRepository : INegocioRepository
    {
        private readonly Conexion _conexion;
        public NegocioRepository(Conexion conexion)
        {
            _conexion = conexion;
        }
        public async Task<Negocio> Obtener()
        {
            Negocio objeto = new Negocio();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_obtenerNegocio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Negocio
                        {
                            RazonSocial = dr["RazonSocial"].ToString(),
                            RFC = dr["RFC"].ToString(),
                            Direccion = dr["Direccion"].ToString(),
                            Celular = dr["Celular"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            SimboloMoneda = dr["SimboloMoneda"].ToString(),
                            NombreLogo = dr["NombreLogo"].ToString(),
                            URL = dr["URL"].ToString()
                        };
                    }
                }
            }
            return objeto;
        }
        public async Task Editar(Negocio objeto)
        {
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_editarNegocio", con);
                cmd.Parameters.Add(new SqlParameter("@RazonSocial", objeto.RazonSocial));
                cmd.Parameters.Add(new SqlParameter("@RFC", objeto.RFC));
                cmd.Parameters.Add(new SqlParameter("@Direccion", objeto.Direccion));
                cmd.Parameters.Add(new SqlParameter("@Celular", objeto.Celular));
                cmd.Parameters.Add(new SqlParameter("@Correo", objeto.Correo));
                cmd.Parameters.Add(new SqlParameter("@SimboloMoneda", objeto.SimboloMoneda));
                cmd.Parameters.Add(new SqlParameter("@NombreLogo", objeto.NombreLogo));
                cmd.Parameters.Add(new SqlParameter("@URL", objeto.URL));
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
