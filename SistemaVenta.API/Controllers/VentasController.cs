using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVServices.Interfaces;
using System.Xml.Linq;
using SistemaVenta.API.Utilidades;
using System.Data;
using ClosedXML.Excel;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly INegocioService _negocioService;
        private readonly IProductoService _productoService;

        public VentasController(IVentaService ventaService, INegocioService negocioService, IProductoService productoService)
        {
            _ventaService = ventaService;
            _negocioService = negocioService;
            _productoService = productoService;
        }

        [HttpGet("GenerarPDF/{numeroVenta}")]
        public async Task<IActionResult> GenerarPDF(string numeroVenta)
        {
            try
            {
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
        public async Task<IActionResult> Obtener(string numeroVenta)
        {
            try
            {
                var v = await _ventaService.Obtener(numeroVenta);
                if (v == null || v.IdVenta == 0)
                {
                    return NotFound($"No se encontró la venta con el número: {numeroVenta}");
                }
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
        public async Task<IActionResult> Historial(string fechaInicio, string fechaFin, string buscar = "")
        {
            try
            {
                var listaEntidades = await _ventaService.Lista(fechaInicio, fechaFin, buscar);

                var listaDto = listaEntidades.Select(v => new VentaDTO
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

        [HttpGet("Detalle/{numeroVenta}")]
        public async Task<IActionResult> Detalle(string numeroVenta)
        {
            try
            {
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

    }
}