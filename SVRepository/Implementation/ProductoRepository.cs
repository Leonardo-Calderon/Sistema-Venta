using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Implementation;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly Conexion _conexion;
        public ProductoRepository(Conexion conexion)
        {
            _conexion = conexion;
        }
        public async Task<String> Crear(Producto objeto)
        {
            string respuesta = "";
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_crearProducto", con);
                cmd.Parameters.Add(new SqlParameter("@IdCategoria", objeto.RefCategoria.IdCategoria));
                cmd.Parameters.Add(new SqlParameter("@Codigo", objeto.Codigo));
                cmd.Parameters.Add(new SqlParameter("@Descripcion", objeto.Descripcion));
                cmd.Parameters.Add(new SqlParameter("@PrecioCompra", objeto.PrecioCompra));
                cmd.Parameters.Add(new SqlParameter("@PrecioVenta", objeto.PrecioVenta));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", objeto.Cantidad));
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

        public async Task<String> Editar(Producto objeto)
        {
            string respuesta = "";
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_editarProducto", con);
                cmd.Parameters.Add(new SqlParameter("@IdProducto", objeto.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@IdCategoria", objeto.RefCategoria.IdCategoria));
                cmd.Parameters.Add(new SqlParameter("@Codigo", objeto.Codigo));
                cmd.Parameters.Add(new SqlParameter("@Descripcion", objeto.Descripcion));
                cmd.Parameters.Add(new SqlParameter("@PrecioCompra", objeto.PrecioCompra));
                cmd.Parameters.Add(new SqlParameter("@PrecioVenta", objeto.PrecioVenta));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", objeto.Cantidad));
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

        public async Task<List<Producto>> Lista(string buscar = "")
        {
            List<Producto> lista = new List<Producto>();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_listaProducto", con);
                cmd.Parameters.Add(new SqlParameter("@Buscar", buscar));
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            RefCategoria = new Categoria
                            {
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Nombre = dr["NombreCategoria"].ToString()
                            },
                            Codigo = dr["Codigo"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                            PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                            Cantidad = Convert.ToInt32(dr["Cantidad"]),
                            Activo = Convert.ToInt32(dr["Activo"]),

                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Producto> Obtener(string codigo)
        {
            Producto objeto = new Producto();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_obtenerProducto", con);
                cmd.Parameters.Add(new SqlParameter("@Codigo", codigo));
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Producto
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            RefCategoria = new Categoria
                            {
                                Nombre = dr["NombreCategoria"].ToString(),
                                RefMedida = new Medida
                                {
                                    Equivalente = dr["Equivalente"].ToString(),
                                    Valor = Convert.ToInt32(dr["Valor"]),
                                }
                            },
                            Codigo = dr["Codigo"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                            Cantidad = Convert.ToInt32(dr["Cantidad"])

                        };
                    }
                }
            }
            return objeto;
        }
    }
}