using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    public class MenuRolRepository: IMenuRolRepository
    {
        private readonly Conexion _conexion;
        public MenuRolRepository(Conexion conexion)
        {
            _conexion = conexion;
        }

        public async Task<List<MenuRol>> Lista(int idRol)
        {
            List<MenuRol> lista = new List<MenuRol>();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_obtenerMenus", con);
                cmd.Parameters.Add(new SqlParameter("@IdRol", idRol));
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new MenuRol
                        {
                            IdMenu = Convert.ToInt32(dr["IdMenu"]), // <-- Línea añadida
                            NombreMenu = dr["NombreMenu"].ToString()!,
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
