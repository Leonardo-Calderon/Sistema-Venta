using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVServices.Interfaces;
using System.Xml.Linq;
// Referencias que vamos a necesitar
using SistemaVenta.API.Utilidades;
using SVRepository.Entities; // Necesaria para llamar a tu método `Util`
using System.IO;
using System.Net.Http;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly INegocioService _negocioService;

        public VentasController(IVentaService ventaService, INegocioService negocioService)
        {
            _ventaService = ventaService;
            _negocioService = negocioService;
        }

        [HttpGet("GenerarPDF/{numeroVenta}")]
        public async Task<IActionResult> GenerarPDF(string numeroVenta)
        {
            try
            {
                // 1. Obtener las ENTIDADES originales, ya que tu método `Util` las necesita.
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

                // Tu método de PDF necesita que la lista de detalles esté dentro del objeto Venta.
                oVenta.RefDetalleVenta = oDetalleVenta;

                // 2. Descargar la imagen del logo
                MemoryStream imagenLogo = new MemoryStream();
                using (var httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(oNegocio.URL))
                    {
                        var imageBytes = await httpClient.GetByteArrayAsync(oNegocio.URL);
                        await imagenLogo.WriteAsync(imageBytes, 0, imageBytes.Length);
                        imagenLogo.Position = 0; // Reseteamos la posición del stream
                    }
                }

                // 3. Llamar a TU método existente para generar el PDF
                var pdfBytes = Util.GeneratePDFVenta(oNegocio, oVenta, imagenLogo);

                // 4. Devolver el archivo
                return File(pdfBytes, "application/pdf", $"Boleta_{numeroVenta}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor al generar el PDF: {ex.Message}");
            }
        }

        // --- MÉTODO AÑADIDO Y CORREGIDO ---
        // Este es el endpoint que faltaba y que causa el error 404.
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

                // Mapeamos la entidad al DTO para enviar solo los datos necesarios al cliente.
                var dto = new VentaDTO
                {
                    IdVenta = v.IdVenta,
                    NumeroVenta = v.NumeroVenta,
                    NombreCliente = v.NombreCliente,
                    PrecioTotal = v.precioTotal,
                    PagoCon = v.PagoCon,
                    Cambio = v.Cambio,
                    FechaRegistro = v.FechaRegistro,
                    NombreUsuario = v.UsuarioRegistrado?.NombreUsuario // Incluimos el nombre del usuario
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

                // --- MAPEO CORREGIDO ---
                // Mapeamos la lista de entidades a una lista de DTOs.
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

                // Mapeamos a DTO para ser consistentes.
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
                var reporte = await _ventaService.Reporte(fechaInicio, fechaFin);
                return Ok(reporte); // Considera mapear esto a un DTO también si es necesario.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}