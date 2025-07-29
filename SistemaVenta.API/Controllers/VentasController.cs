using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVServices.Interfaces;
using System.Xml.Linq;
using SistemaVenta.API.Utilidades;
using System.Data;
using ClosedXML.Excel;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Autorización a nivel de controlador
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly INegocioService _negocioService;
        private readonly IProductoService _productoService;
        private readonly IAuditoriaService _auditoriaService;
        private readonly IValidacionService _validacionService;
        private readonly ILogger<VentasController> _logger;

        public VentasController(IVentaService ventaService, INegocioService negocioService, IProductoService productoService, IAuditoriaService auditoriaService, IValidacionService validacionService, ILogger<VentasController> logger)
        {
            _ventaService = ventaService;
            _negocioService = negocioService;
            _productoService = productoService;
            _auditoriaService = auditoriaService;
            _validacionService = validacionService;
            _logger = logger;
        }

        [HttpGet("GenerarPDF/{numeroVenta}")]
        [Authorize(Roles = "Administrador,Vendedor")] // Administradores y vendedores pueden generar PDFs
        public async Task<IActionResult> GenerarPDF(string numeroVenta)
        {
            try
            {
                // Extraer el ID del usuario del token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido o no contiene el ID del usuario.");
                }

                // Verificar si el usuario es administrador
                var isAdmin = User.IsInRole("Administrador");

                var negocioTask = _negocioService.Obtener();
                var ventaTask = _ventaService.Obtener(numeroVenta);
                var detalleTask = _ventaService.ObtenerDetalle(numeroVenta);

                await Task.WhenAll(negocioTask, ventaTask, detalleTask);

                var oNegocio = negocioTask.Result;
                var oVenta = ventaTask.Result;
                var oDetalleVenta = detalleTask.Result;

                if (oVenta == null || oVenta.IdVenta == 0)
                {
                    return NotFound($"Venta {numeroVenta} no encontrada.");
                }

                // Verificar propiedad del recurso
                if (!isAdmin && oVenta.UsuarioRegistrado?.IdUsuario != userId)
                {
                    return Forbid("No tiene permisos para generar el PDF de esta venta.");
                }

                oVenta.RefDetalleVenta = oDetalleVenta;

                MemoryStream imagenLogo = new MemoryStream();
                using (var httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(oNegocio.URL))
                    {
                        var imageBytes = await httpClient.GetByteArrayAsync(oNegocio.URL);
                        await imagenLogo.WriteAsync(imageBytes, 0, imageBytes.Length);
                        imagenLogo.Position = 0;
                    }
                }
                var pdfBytes = Util.GeneratePDFVenta(oNegocio, oVenta, imagenLogo);

                return File(pdfBytes, "application/pdf", $"Boleta_{numeroVenta}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor al generar el PDF: {ex.Message}");
            }
        }

        [HttpGet("Obtener/{numeroVenta}")]
        [Authorize(Roles = "Administrador,Vendedor")] // Administradores y vendedores pueden ver ventas
        public async Task<IActionResult> Obtener(string numeroVenta)
        {
            try
            {
                // Extraer el ID del usuario del token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido o no contiene el ID del usuario.");
                }

                // Verificar si el usuario es administrador
                var isAdmin = User.IsInRole("Administrador");

                var v = await _ventaService.Obtener(numeroVenta);
                if (v == null || v.IdVenta == 0)
                {
                    return NotFound($"No se encontró la venta con el número: {numeroVenta}");
                }

                // Verificar propiedad del recurso
                if (!isAdmin && v.UsuarioRegistrado?.IdUsuario != userId)
                {
                    // Registrar autorización denegada
                    await _auditoriaService.RegistrarAutorizacion(User, $"Venta {numeroVenta}", "Consulta", "Denegado", "Usuario no es propietario de la venta");
                    return Forbid("No tiene permisos para acceder a esta venta.");
                }

                // Registrar autorización exitosa
                await _auditoriaService.RegistrarAutorizacion(User, $"Venta {numeroVenta}", "Consulta", "Permitido", isAdmin ? "Usuario es Administrador" : "Usuario es propietario de la venta");

                var dto = new VentaDTO
                {
                    IdVenta = v.IdVenta,
                    NumeroVenta = v.NumeroVenta,
                    NombreCliente = v.NombreCliente,
                    PrecioTotal = v.precioTotal,
                    PagoCon = v.PagoCon,
                    Cambio = v.Cambio,
                    FechaRegistro = v.FechaRegistro,
                    NombreUsuario = v.UsuarioRegistrado?.NombreUsuario
                };
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("Historial")]
        [Authorize(Roles = "Administrador,Vendedor")] // Administradores y vendedores pueden ver historial
        public async Task<IActionResult> Historial(string fechaInicio, string fechaFin, string buscar = "")
        {
            try
            {
                // Extraer el ID del usuario del token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido o no contiene el ID del usuario.");
                }

                // Verificar si el usuario es administrador
                var isAdmin = User.IsInRole("Administrador");

                var listaEntidades = await _ventaService.Lista(fechaInicio, fechaFin, buscar);

                // Filtrar ventas según el rol del usuario
                var ventasFiltradas = listaEntidades;
                if (!isAdmin)
                {
                    // Si no es administrador, solo mostrar sus propias ventas
                    ventasFiltradas = listaEntidades.Where(v => v.UsuarioRegistrado?.IdUsuario == userId).ToList();
                }

                var listaDto = ventasFiltradas.Select(v => new VentaDTO
                {
                    NumeroVenta = v.NumeroVenta,
                    NombreCliente = v.NombreCliente,
                    PrecioTotal = v.precioTotal,
                    FechaRegistro = v.FechaRegistro,
                    NombreUsuario = v.UsuarioRegistrado?.NombreUsuario
                }).ToList();

                return Ok(listaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// PASO 4: Endpoint de búsqueda segura de ventas con validación y sanitización
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = "Administrador,Vendedor")]
        public async Task<IActionResult> BusquedaSegura([FromQuery] string searchTerm = "", [FromQuery] string fechaInicio = "", [FromQuery] string fechaFin = "")
        {
            try
            {
                // PASO 4: Validación y sanitización del término de búsqueda
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    _logger.LogInformation("Búsqueda de ventas sin término de búsqueda");
                    return await Historial(fechaInicio, fechaFin, ""); // Retornar historial completo
                }

                // Sanitizar el término de búsqueda
                var searchTermSanitizado = _validacionService.SanitizarString(searchTerm, 50, true);
                
                // Verificar si el término de búsqueda fue rechazado por la sanitización
                if (searchTermSanitizado == null)
                {
                    _logger.LogWarning("Término de búsqueda de ventas rechazado por sanitización: {SearchTerm}", searchTerm);
                    return BadRequest("El término de búsqueda contiene caracteres no permitidos.");
                }

                // Verificar si contiene caracteres peligrosos
                if (_validacionService.ContieneCaracteresPeligrosos(searchTerm))
                {
                    _logger.LogWarning("Se detectaron caracteres peligrosos en la búsqueda de ventas: {SearchTerm}", searchTerm);
                    return BadRequest("El término de búsqueda contiene caracteres no permitidos.");
                }

                // Validar longitud mínima y máxima
                if (searchTermSanitizado.Length < 2)
                {
                    _logger.LogWarning("Término de búsqueda de ventas demasiado corto: {SearchTerm}", searchTermSanitizado);
                    return BadRequest("El término de búsqueda debe tener al menos 2 caracteres.");
                }

                if (searchTermSanitizado.Length > 50)
                {
                    _logger.LogWarning("Término de búsqueda de ventas demasiado largo: {SearchTerm}", searchTermSanitizado);
                    return BadRequest("El término de búsqueda no puede exceder 50 caracteres.");
                }

                // Extraer el ID del usuario del token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido o no contiene el ID del usuario.");
                }

                // Verificar si el usuario es administrador
                var isAdmin = User.IsInRole("Administrador");

                // PASO 4: Realizar búsqueda segura usando el servicio
                _logger.LogInformation("Iniciando búsqueda segura de ventas con término: {SearchTerm}", searchTermSanitizado);
                
                var listaEntidades = await _ventaService.Lista(fechaInicio, fechaFin, searchTermSanitizado);
                
                // Aplicar filtro de propiedad del recurso
                var ventasFiltradas = listaEntidades;
                if (!isAdmin)
                {
                    ventasFiltradas = listaEntidades.Where(v => v.UsuarioRegistrado?.IdUsuario == userId).ToList();
                }

                // Mapear a DTOs
                var listaDto = ventasFiltradas.Select(v => new VentaDTO
                {
                    NumeroVenta = v.NumeroVenta,
                    NombreCliente = v.NombreCliente,
                    PrecioTotal = v.precioTotal,
                    FechaRegistro = v.FechaRegistro,
                    NombreUsuario = v.UsuarioRegistrado?.NombreUsuario
                }).ToList();

                _logger.LogInformation("Búsqueda segura de ventas completada. Resultados encontrados: {Count}", listaDto.Count);
                
                return Ok(new
                {
                    TerminoBusqueda = searchTermSanitizado,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    TotalResultados = listaDto.Count,
                    Resultados = listaDto,
                    FechaBusqueda = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en búsqueda segura de ventas con término: {SearchTerm}", searchTerm);
                return StatusCode(500, "Error interno del servidor durante la búsqueda.");
            }
        }

        [HttpGet("Detalle/{numeroVenta}")]
        [Authorize(Roles = "Administrador,Vendedor")] // Administradores y vendedores pueden ver detalles
        public async Task<IActionResult> Detalle(string numeroVenta)
        {
            try
            {
                // Extraer el ID del usuario del token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido o no contiene el ID del usuario.");
                }

                // Verificar si el usuario es administrador
                var isAdmin = User.IsInRole("Administrador");

                // Primero obtener la venta para verificar propiedad
                var venta = await _ventaService.Obtener(numeroVenta);
                if (venta == null || venta.IdVenta == 0)
                {
                    return NotFound($"No se encontró la venta con el número: {numeroVenta}");
                }

                // Verificar propiedad del recurso
                if (!isAdmin && venta.UsuarioRegistrado?.IdUsuario != userId)
                {
                    return Forbid("No tiene permisos para acceder al detalle de esta venta.");
                }

                var listaEntidad = await _ventaService.ObtenerDetalle(numeroVenta);

                var listaDto = listaEntidad.Select(d => new DetalleVentaDTO
                {
                    DescripcionProducto = d.RefProducto.Descripcion,
                    Cantidad = d.Cantidad,
                    Precio = d.PrecioVenta,
                    Total = d.PrecioTotal
                }).ToList();

                return Ok(listaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("Registrar")]
        [Authorize(Roles = "Administrador,Vendedor")] // Administradores y vendedores pueden registrar ventas
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            if (venta == null || venta.DetalleVenta == null || !venta.DetalleVenta.Any())
            {
                return BadRequest("Venta inválida");
            }

            try
            {
                XElement ventaXml = new XElement("Venta",
                    new XElement("IdUsuarioRegistro", venta.IdUsuarioRegistro),
                    new XElement("NombreCliente", venta.NombreCliente),
                    new XElement("PrecioTotal", venta.PrecioTotal),
                    new XElement("PagoCon", venta.PagoCon),
                    new XElement("Cambio", venta.Cambio)
                );

                XElement detalleVenta = new XElement("DetalleVenta");
                foreach (var item in venta.DetalleVenta)
                {
                    detalleVenta.Add(new XElement("Item",
                        new XElement("IdProducto", item.IdProducto),
                        new XElement("Cantidad", item.Cantidad),
                        new XElement("PrecioVenta", item.Precio),
                        new XElement("PrecioTotal", item.Total)
                    ));
                }
                ventaXml.Add(detalleVenta);

                var numeroVenta = await _ventaService.Registrar(ventaXml.ToString());
                if (string.IsNullOrEmpty(numeroVenta))
                {
                    return StatusCode(500, "Error al registrar la venta.");
                }

                return Ok(new { numeroVenta });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("Reporte")]
        [Authorize(Roles = "Administrador")] // Solo administradores pueden ver reportes
        public async Task<IActionResult> Reporte(string fechaInicio, string fechaFin)
        {
            try
            {
                var listaEntidad = await _ventaService.Reporte(fechaInicio, fechaFin);

                if (listaEntidad == null)
                {
                    return Ok(new List<ReporteVentaDTO>());
                }

                var listaDto = listaEntidad.Select(d => new ReporteVentaDTO
                {
                    NumeroVenta = d.RefVenta.NumeroVenta,
                    NombreUsuario = d.RefVenta.UsuarioRegistrado.NombreUsuario,
                    FechaRegistro = d.RefVenta.FechaRegistro,
                    Producto = d.RefProducto.Descripcion,
                    PrecioCompra = d.RefProducto.PrecioCompra,
                    PrecioVenta = d.PrecioVenta,
                    Cantidad = d.Cantidad,
                    PrecioTotal = d.PrecioTotal
                }).ToList();

                // Devolvemos la lista directamente, como en tus otros métodos.
                return Ok(listaDto);
            }
            catch (Exception ex)
            {
                // Devolvemos un error 500 con el mensaje.
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("GenerarReporteExcel")]
        [Authorize(Roles = "Administrador")] // Solo administradores pueden generar reportes Excel
        [ValidateAntiForgeryToken]
        public IActionResult GenerarReporteExcel([FromBody] List<ReporteVentaDTO> listaReporte)
        {
            try
            {
                using (var wb = new XLWorkbook())
                {
                    var dt = new DataTable();
                    dt.TableName = "Reporte de Ventas";
                    dt.Columns.Add("Numero Venta", typeof(string));
                    dt.Columns.Add("Nombre Usuario", typeof(string));
                    dt.Columns.Add("Fecha Registro", typeof(string));
                    dt.Columns.Add("Producto", typeof(string));
                    dt.Columns.Add("Precio Compra", typeof(decimal));
                    dt.Columns.Add("Precio Venta", typeof(decimal));
                    dt.Columns.Add("Cantidad", typeof(int));
                    dt.Columns.Add("Precio Total", typeof(decimal));

                    foreach (var item in listaReporte)
                    {
                        dt.Rows.Add(
                            item.NumeroVenta,
                            item.NombreUsuario,
                            item.FechaRegistro,
                            item.Producto,
                            item.PrecioCompra,
                            item.PrecioVenta,
                            item.Cantidad,
                            item.PrecioTotal
                        );
                    }
                    var hojaDatos = wb.Worksheets.Add(dt);
                    hojaDatos.ColumnsUsed().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        // Devolvemos los bytes del archivo directamente.
                        return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("Lista")]
        [Authorize(Roles = "Administrador,Vendedor")] // Administradores y vendedores pueden ver lista de productos
        public async Task<IActionResult> Lista()
        {
            try
            {
                
                var lista = await _productoService.Lista();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("TestAcceso/{numeroVenta}")]
        [Authorize(Roles = "Administrador,Vendedor")] // Endpoint de prueba para verificar acceso
        public async Task<IActionResult> TestAcceso(string numeroVenta)
        {
            try
            {
                // Extraer el ID del usuario del token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido o no contiene el ID del usuario.");
                }

                // Verificar si el usuario es administrador
                var isAdmin = User.IsInRole("Administrador");

                // Obtener información del usuario actual
                var nombreUsuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido";
                var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value ?? "Sin rol";

                var v = await _ventaService.Obtener(numeroVenta);
                if (v == null || v.IdVenta == 0)
                {
                    return NotFound($"No se encontró la venta con el número: {numeroVenta}");
                }

                // Verificar propiedad del recurso
                if (!isAdmin && v.UsuarioRegistrado?.IdUsuario != userId)
                {
                    return Forbid($"No tiene permisos para acceder a esta venta. Venta pertenece al usuario ID: {v.UsuarioRegistrado?.IdUsuario}, su ID: {userId}");
                }

                // Respuesta de prueba con información detallada
                var resultadoPrueba = new
                {
                    Mensaje = "Acceso permitido - Prueba exitosa",
                    UsuarioActual = new
                    {
                        Id = userId,
                        Nombre = nombreUsuario,
                        Rol = rolUsuario,
                        EsAdministrador = isAdmin
                    },
                    Venta = new
                    {
                        NumeroVenta = v.NumeroVenta,
                        NombreCliente = v.NombreCliente,
                        PrecioTotal = v.precioTotal,
                        FechaRegistro = v.FechaRegistro,
                        UsuarioRegistrado = v.UsuarioRegistrado?.NombreUsuario,
                        IdUsuarioRegistrado = v.UsuarioRegistrado?.IdUsuario
                    },
                    Verificacion = new
                    {
                        PropiedadVerificada = true,
                        MotivoAcceso = isAdmin ? "Usuario es Administrador" : "Usuario es propietario de la venta",
                        Timestamp = DateTime.UtcNow
                    }
                };

                return Ok(resultadoPrueba);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}