
using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    /// <summary>
    /// Implementación del repositorio para la gestión de ventas del sistema.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IVentaRepository y proporciona métodos
    /// para acceder a los datos de ventas almacenados en la base de datos SQL Server.
    /// Las ventas son el núcleo del sistema de gestión comercial e incluyen funcionalidades
    /// para registro de ventas, consulta de detalles y generación de reportes.
    /// </remarks>
    public class VentaRepository : IVentaRepository
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private readonly Conexion _conexion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase VentaRepository.
        /// </summary>
        /// <param name="conexion">Instancia de la clase Conexion para acceder a la base de datos.</param>
        /// <remarks>
        /// El constructor recibe una instancia de Conexion que será utilizada
        /// para establecer conexiones a la base de datos en todas las operaciones.
        /// </remarks>
        public VentaRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        /// <summary>
        /// Registra una nueva venta en el sistema a partir de un XML que contiene la información de la venta.
        /// </summary>
        /// <param name="ventaXML">Cadena XML que contiene la información completa de la venta y sus detalles.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el número de venta generado o un mensaje de error.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_registrarVenta' que procesa un XML que contiene la información
        /// de la venta y sus detalles, registra la venta en la base de datos y retorna el número de venta generado automáticamente.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el XML de venta es null o vacío.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<string> Registrar(string ventaXML)
        {
            if (string.IsNullOrWhiteSpace(ventaXML))
                throw new ArgumentNullException(nameof(ventaXML));

            string respuesta = "";
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_registrarVenta", con);
                cmd.Parameters.AddWithValue("@VentaXML", ventaXML);
                cmd.Parameters.Add("@NumeroVenta", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@NumeroVenta"].Value) ?? string.Empty;
                }
                catch (Exception ex)
                {
                    respuesta = $"Error al registrar la venta: {ex.Message}";
                }
            }
            
            return respuesta;
        }

        /// <summary>
        /// Obtiene la información completa de una venta específica por su número de venta.
        /// </summary>
        /// <param name="numeroVenta">Número único de la venta a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es la venta encontrada o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_ObtenerVenta' que busca una venta específica por su número
        /// y retorna la información completa incluyendo datos del cliente y usuario que registró la venta.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el número de venta es null o vacío.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<Venta> Obtener(string numeroVenta)
        {
            if (string.IsNullOrWhiteSpace(numeroVenta))
                throw new ArgumentNullException(nameof(numeroVenta));

            Venta objeto = new Venta();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
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
                            NumeroVenta = dr["NumeroVenta"].ToString() ?? string.Empty,
                            UsuarioRegistrado = new Usuario
                            {
                                NombreUsuario = dr["NombreUsuario"].ToString() ?? string.Empty,
                            },
                            NombreCliente = dr["NombreCliente"].ToString() ?? string.Empty,
                            precioTotal = Convert.ToDecimal(dr["PrecioTotal"]),
                            Cambio = Convert.ToDecimal(dr["Cambio"]),
                            FechaRegistro = dr["FechaRegistro"].ToString() ?? string.Empty,
                        };
                    }
                }
            }
            
            return objeto;
        }

        /// <summary>
        /// Obtiene los detalles de una venta específica por su número de venta.
        /// </summary>
        /// <param name="numeroVenta">Número único de la venta cuyos detalles se quieren obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de detalles de la venta.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_ObtenerDetalleVenta' que retorna todos los productos vendidos
        /// en una venta específica, incluyendo cantidades, precios y información del producto.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el número de venta es null o vacío.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<DetalleVenta>> ObtenerDetalle(string numeroVenta)
        {
            if (string.IsNullOrWhiteSpace(numeroVenta))
                throw new ArgumentNullException(nameof(numeroVenta));

            List<DetalleVenta> lista = new List<DetalleVenta>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
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
                                Descripcion = dr["Descripcion"].ToString() ?? string.Empty,
                                RefCategoria = new Categoria
                                {
                                    RefMedida = new Medida
                                    {
                                        Abreviatura = dr["Abreviatura"].ToString() ?? string.Empty,
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

        /// <summary>
        /// Obtiene una lista de ventas filtrada por rango de fechas y opcionalmente por término de búsqueda.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del rango de búsqueda en formato string.</param>
        /// <param name="fechaFin">Fecha de fin del rango de búsqueda en formato string.</param>
        /// <param name="buscar">Término opcional para filtrar las ventas por número de venta o nombre de cliente.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de ventas que cumplen con los criterios.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_listaVenta' que retorna todas las ventas realizadas
        /// en el rango de fechas especificado, opcionalmente filtradas por el término de búsqueda.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando las fechas son null o vacías.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<Venta>> Lista(string fechaInicio, string fechaFin, string buscar = "")
        {
            if (string.IsNullOrWhiteSpace(fechaInicio))
                throw new ArgumentNullException(nameof(fechaInicio));
            if (string.IsNullOrWhiteSpace(fechaFin))
                throw new ArgumentNullException(nameof(fechaFin));

            List<Venta> lista = new List<Venta>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_listaVenta", con);
                cmd.Parameters.Add(new SqlParameter("@FechaInicio", fechaInicio));
                cmd.Parameters.Add(new SqlParameter("@FechaFin", fechaFin));
                cmd.Parameters.Add(new SqlParameter("@Buscar", buscar ?? string.Empty));
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Venta
                        {
                            NumeroVenta = dr["NumeroVenta"].ToString() ?? string.Empty,
                            UsuarioRegistrado = new Usuario
                            {
                                NombreUsuario = dr["NombreUsuario"].ToString() ?? string.Empty,
                            },
                            NombreCliente = dr["NombreCliente"].ToString() ?? string.Empty,
                            precioTotal = Convert.ToDecimal(dr["PrecioTotal"]),
                            PagoCon = Convert.ToDecimal(dr["PagoCon"]),
                            Cambio = Convert.ToDecimal(dr["Cambio"]),
                            FechaRegistro = dr["FechaRegistro"].ToString() ?? string.Empty
                        });
                    }
                }
            }
            
            return lista;
        }

        /// <summary>
        /// Genera un reporte de ventas para un rango de fechas específico.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del reporte en formato string.</param>
        /// <param name="fechaFin">Fecha de fin del reporte en formato string.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de detalles de ventas para el reporte.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_reporteVenta' que genera un reporte detallado de todas las ventas
        /// realizadas en el rango de fechas especificado, incluyendo información de productos, precios y ganancias.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando las fechas son null o vacías.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            if (string.IsNullOrWhiteSpace(fechaInicio))
                throw new ArgumentNullException(nameof(fechaInicio));
            if (string.IsNullOrWhiteSpace(fechaFin))
                throw new ArgumentNullException(nameof(fechaFin));

            List<DetalleVenta> lista = new List<DetalleVenta>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_reporteVenta", con);
                cmd.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = fechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = fechaFin;
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new DetalleVenta
                        {
                            RefVenta = new Venta
                            {
                                NumeroVenta = dr["NumeroVenta"].ToString() ?? string.Empty,
                                UsuarioRegistrado = new Usuario
                                {
                                    NombreUsuario = dr["NombreUsuario"].ToString() ?? string.Empty,
                                },
                                FechaRegistro = dr["FechaRegistro"].ToString() ?? string.Empty
                            },
                            RefProducto = new Producto
                            {
                                Descripcion = dr["Producto"].ToString() ?? string.Empty,
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                            },
                            PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                            Cantidad = Convert.ToInt32(dr["Cantidad"]),
                            PrecioTotal = Convert.ToDecimal(dr["PrecioTotal"]),
                        });
                    }
                }
            }
            
            return lista;
        }
    }
}
