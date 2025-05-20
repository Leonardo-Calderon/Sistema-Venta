
using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    public class VentaRepository : IVentaRepository
    {
        private readonly Conexion _conexion;
        public VentaRepository(Conexion conexion)
        {
            _conexion = conexion;
        }
        public async Task<string> Registrar(string ventaXML)
        {
            string respuesta = "";
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_registrarVenta", con);
                cmd.Parameters.AddWithValue("@VentaXML", ventaXML);
                cmd.Parameters.Add("@NumeroVenta", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@NumeroVenta"].Value)!;
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }

            }
            return respuesta;
        }
        public async Task<Venta> Obtener(string numeroVenta)
        {
            Venta objeto = new Venta();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_ObtenerVenta", con);
                cmd.Parameters.AddWithValue("@NumeroVenta", numeroVenta);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Venta
                        {
                            IdVenta = Convert.ToInt32(dr["IdVenta"]),
                            NumeroVenta = dr["NumeroVenta"].ToString()!,
                            UsuarioRegistrado = new Usuario
                            {
                                NombreUsuario = dr["NombreUsuario"].ToString()!,
                            },
                            NombreCliente = dr["NombreCliente"].ToString()!,
                            precioTotal = Convert.ToDecimal(dr["PrecioTotal"]),
                            Cambio = Convert.ToDecimal(dr["Cambio"]),
                            FechaRegistro = dr["FechaRegistro"].ToString()!,
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<List<DetalleVenta>> ObtenerDetalle(string numeroVenta)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_ObtenerDetalleVenta", con);
                cmd.Parameters.Add(new SqlParameter("@NumeroVenta", numeroVenta));
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new DetalleVenta
                        {
                            RefProducto = new Producto
                            {
                                Descripcion = dr["Descripcion"].ToString()!,
                                RefCategoria = new Categoria
                                {
                                    RefMedida = new Medida
                                    {
                                        Abreviatura = dr["Abreviatura"].ToString()!,
                                        Valor = dr["Valor"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Valor"]),
                                    }
                                }
                            },
                            Cantidad = dr["Cantidad"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Cantidad"]),
                            PrecioVenta = dr["PrecioVenta"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PrecioVenta"]),
                            PrecioTotal = dr["PrecioTotal"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PrecioTotal"]),
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<List<Venta>> Lista(string fechaInicio, string fechaFin, string buscar = "")
        {
            List<Venta> lista = new List<Venta>();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_listaVenta", con);
                cmd.Parameters.Add(new SqlParameter("@FechaInicio", fechaInicio));
                cmd.Parameters.Add(new SqlParameter("@FechaFin", fechaFin));
                cmd.Parameters.Add(new SqlParameter("@Buscar", buscar));
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Venta
                        {
                            NumeroVenta = dr["NumeroVenta"].ToString()!,
                            UsuarioRegistrado = new Usuario
                            {
                                NombreUsuario = dr["NombreUsuario"].ToString()!,
                            },
                            NombreCliente = dr["NombreCliente"].ToString()!,
                            precioTotal = Convert.ToDecimal(dr["PrecioTotal"]),
                            PagoCon = Convert.ToDecimal(dr["PagoCon"]),
                            Cambio = Convert.ToDecimal(dr["Cambio"]),
                            FechaRegistro = dr["FechaRegistro"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();
            using (var con = _conexion.ObtenerSQLConexion())
            {
                con.Open();
                var cmd = new SqlCommand("sp_reporteVenta", con);
                cmd.Parameters.Add(new SqlParameter("@FechaInicio", fechaInicio));
                cmd.Parameters.Add(new SqlParameter("@FechaFin", fechaFin));
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new DetalleVenta
                        {
                            RefVenta = new Venta
                            {
                                NumeroVenta = dr["NumeroVenta"].ToString()!,
                                UsuarioRegistrado = new Usuario
                                {
                                    NombreUsuario = dr["NombreUsuario"].ToString()!,
                                },
                                FechaRegistro = dr["FechaRegistro"].ToString()!
                            },
                            RefProducto = new Producto
                            {
                                Descripcion = dr["Producto"].ToString()!,
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                            },
                            PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                            Cantidad = Convert.ToInt32(dr["Cantidad"]),
                            PrecioTotal = Convert.ToDecimal(dr["PrecioTotal"]),
                        });
                    }
                }
            }
            return lista; throw new NotImplementedException();
        }
    }
}
