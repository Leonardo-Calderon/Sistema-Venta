// En: SistemaVenta.API/Controllers/ReportesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVServices.Interfaces;
using System.Globalization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly IVentaService _ventaService;

    public ReportesController(IVentaService ventaService)
    {
        _ventaService = ventaService;
    }

    // GET: api/reportes?fechaInicio=2024-01-01&fechaFin=2024-01-31
    [HttpGet]
    public async Task<IActionResult> Venta([FromQuery] string fechaInicio, [FromQuery] string fechaFin)
    {
        try
        {
            // Nota: El servicio espera las fechas como strings, así que las pasamos directamente.
            var listaEntidades = await _ventaService.Reporte(fechaInicio, fechaFin);

            if (listaEntidades == null || !listaEntidades.Any())
            {
                return Ok(new List<ReporteVentaDTO>());
            }

            // Mapeamos la lista de entidades (DetalleVenta) a una lista de DTOs para el reporte.
            var listaDto = listaEntidades.Select(detalle => new ReporteVentaDTO
            {
                NumeroVenta = detalle.RefVenta?.NumeroVenta,
                NombreUsuario = detalle.RefVenta?.UsuarioRegistrado?.NombreUsuario,
                FechaRegistro = DateTime.TryParse(detalle.RefVenta?.FechaRegistro, out var fecha) ? fecha : (DateTime?)null,
                Producto = detalle.RefProducto?.Descripcion,
                PrecioCompra = detalle.RefProducto?.PrecioCompra ?? 0,
                PrecioVenta = detalle.PrecioVenta,
                Cantidad = detalle.Cantidad,
                PrecioTotal = detalle.PrecioTotal
            }).ToList();

            return Ok(listaDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}