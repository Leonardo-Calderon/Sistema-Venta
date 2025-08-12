using Microsoft.Data.SqlClient;
using SVRepository.DB;
using SVRepository.Entities;
using SVRepository.Implementation;
using SVRepository.Interfaces;
using System.Data;

namespace SVRepository.Implementation
{
    /// <summary>
    /// Implementación del repositorio para la gestión de productos del sistema de ventas.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IProductoRepository y proporciona métodos
    /// para acceder a los datos de productos almacenados en la base de datos SQL Server.
    /// Los productos son los elementos que se venden en el sistema y incluyen información
    /// sobre precios, stock, categoría y estado de activación.
    /// </remarks>
    public class ProductoRepository : IProductoRepository
    {
        /// <summary>
        /// Instancia de la clase de conexión a la base de datos.
        /// </summary>
        private readonly Conexion _conexion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase ProductoRepository.
        /// </summary>
        /// <param name="conexion">Instancia de la clase Conexion para acceder a la base de datos.</param>
        /// <remarks>
        /// El constructor recibe una instancia de Conexion que será utilizada
        /// para establecer conexiones a la base de datos en todas las operaciones.
        /// </remarks>
        public ProductoRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Producto con los datos del nuevo producto a crear.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_crearProducto' que valida los datos del producto
        /// y lo inserta en la base de datos. Incluye validación de conexión y manejo robusto de errores.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el objeto producto es null.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error específico de la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<string> Crear(Producto objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            string respuesta = "";
            
            try
            {
                using (var con = _conexion.ObtenerSQLConexion())
                {
                    await con.OpenAsync();
                    
                    // Verificar que la conexión esté abierta
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        return "Error: No se pudo abrir la conexión a la base de datos";
                    }

                    var cmd = new SqlCommand("sp_crearProducto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    // Agregar parámetros con validación
                    cmd.Parameters.Add(new SqlParameter("@IdCategoria", objeto.RefCategoria?.IdCategoria ?? 0));
                    cmd.Parameters.Add(new SqlParameter("@Codigo", objeto.Codigo ?? string.Empty));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", objeto.Descripcion ?? string.Empty));
                    cmd.Parameters.Add(new SqlParameter("@PrecioCompra", objeto.PrecioCompra));
                    cmd.Parameters.Add(new SqlParameter("@PrecioVenta", objeto.PrecioVenta));
                    cmd.Parameters.Add(new SqlParameter("@Cantidad", objeto.Cantidad));
                    cmd.Parameters.Add("@MsjError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@MsjError"].Value) ?? string.Empty;
                }
            }
            catch (SqlException ex)
            {
                respuesta = $"Error de base de datos: {ex.Message} (Error #{ex.Number})";
            }
            catch (Exception ex)
            {
                respuesta = $"Error inesperado: {ex.Message}";
            }

            return respuesta;
        }

        /// <summary>
        /// Actualiza los datos de un producto existente en el sistema.
        /// </summary>
        /// <param name="objeto">Objeto Producto con los datos actualizados del producto.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un mensaje indicando el éxito o error de la operación.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_editarProducto' que actualiza los datos de un producto existente,
        /// incluyendo su estado activo/inactivo. Retorna un mensaje de confirmación si la operación es exitosa,
        /// o un mensaje de error en caso contrario.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el objeto producto es null.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<string> Editar(Producto objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            string respuesta = "";
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_editarProducto", con);
                cmd.Parameters.Add(new SqlParameter("@IdProducto", objeto.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@IdCategoria", objeto.RefCategoria?.IdCategoria ?? 0));
                cmd.Parameters.Add(new SqlParameter("@Codigo", objeto.Codigo ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@Descripcion", objeto.Descripcion ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@PrecioCompra", objeto.PrecioCompra));
                cmd.Parameters.Add(new SqlParameter("@PrecioVenta", objeto.PrecioVenta));
                cmd.Parameters.Add(new SqlParameter("@Cantidad", objeto.Cantidad));
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
                    respuesta = $"Error al editar el producto: {ex.Message}";
                }
            }
            
            return respuesta;
        }

        /// <summary>
        /// Obtiene una lista de productos opcionalmente filtrada por un término de búsqueda.
        /// </summary>
        /// <param name="buscar">Término opcional para filtrar los productos por código o descripción. Si está vacío, retorna todos los productos.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de productos.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_listaProducto' que retorna todos los productos activos
        /// o aquellos que coincidan con el término de búsqueda proporcionado. Incluye información de la categoría.
        /// </remarks>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<List<Producto>> Lista(string buscar = "")
        {
            List<Producto> lista = new List<Producto>();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
                var cmd = new SqlCommand("sp_listaProducto", con);
                cmd.Parameters.Add(new SqlParameter("@Buscar", buscar ?? string.Empty));
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
                                Nombre = dr["NombreCategoria"].ToString() ?? string.Empty
                            },
                            Codigo = dr["Codigo"].ToString() ?? string.Empty,
                            Descripcion = dr["Descripcion"].ToString() ?? string.Empty,
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

        /// <summary>
        /// Obtiene un producto específico por su código.
        /// </summary>
        /// <param name="codigo">Código único del producto a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el producto encontrado o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método ejecuta el stored procedure 'sp_obtenerProducto' que busca un producto específico por su código.
        /// Retorna el producto completo con su información de categoría y medida si existe.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se lanza cuando el código es null o vacío.</exception>
        /// <exception cref="SqlException">Se lanza cuando ocurre un error en la base de datos.</exception>
        /// <exception cref="Exception">Se lanza cuando ocurre un error inesperado durante la operación.</exception>
        public async Task<Producto> Obtener(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentNullException(nameof(codigo));

            Producto objeto = new Producto();
            
            using (var con = _conexion.ObtenerSQLConexion())
            {
                await con.OpenAsync();
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
                                Nombre = dr["NombreCategoria"].ToString() ?? string.Empty,
                                RefMedida = new Medida
                                {
                                    Equivalente = dr["Equivalente"].ToString() ?? string.Empty,
                                    Valor = Convert.ToInt32(dr["Valor"]),
                                }
                            },
                            Codigo = dr["Codigo"].ToString() ?? string.Empty,
                            Descripcion = dr["Descripcion"].ToString() ?? string.Empty,
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