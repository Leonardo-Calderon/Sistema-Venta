// En: SistemaVenta.API/Controllers/VentasController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVServices.Interfaces;
using System.Security.Claims;
using System.Xml.Linq; // Necesario para crear el XML

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventaService;

    public VentasController(IVentaService ventaService)
    {
        _ventaService = ventaService;
    }

    // POST: api/ventas/registrar
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] VentaDTO dto)
    {
        if (dto == null || !dto.Detalles.Any())
        {
            return BadRequest("La venta debe contener al menos un producto.");
        }

        try
        {
            // 1. Obtenemos el ID del usuario que está registrando la venta desde el token.
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("No se pudo identificar al usuario.");
            }
            int idUsuario = int.Parse(userIdClaim.Value);

            // 2. Construimos el XML que espera el procedimiento almacenado.
            var ventaXml = new XElement("Venta",
                new XElement("Usuario",
                    new XAttribute("IdUsuario", idUsuario),
                    new XAttribute("NombreCliente", dto.NombreCliente ?? ""), // Usamos ?? "" por si es nulo
                    new XAttribute("PrecioTotal", dto.PrecioTotal),
                    new XAttribute("PagoCon", dto.PagoCon),
                    new XAttribute("Cambio", dto.Cambio)
                ),
                new XElement("DetalleVenta",
                    from detalle in dto.Detalles
                    select new XElement("Producto",
                        new XAttribute("IdProducto", detalle.IdProducto),
                        new XAttribute("Cantidad", detalle.Cantidad),
                        new XAttribute("PrecioVenta", detalle.Precio),
                        new XAttribute("PrecioTotal", detalle.Total)
                    )
                )
            ).ToString();

            // 3. Llamamos al servicio con el XML.
            string numeroVentaGenerado = await _ventaService.Registrar(ventaXml);

            if (string.IsNullOrEmpty(numeroVentaGenerado) || numeroVentaGenerado.Contains("Error"))
            {
                return BadRequest($"No se pudo registrar la venta: {numeroVentaGenerado}");
            }

            // Devolvemos el número de venta para que el cliente pueda mostrarlo.
            return Ok(new { numeroVenta = numeroVentaGenerado });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // GET: api/ventas?fechaInicio=...&fechaFin=...&buscar=...
    [HttpGet]
    public async Task<IActionResult> Historial([FromQuery] string fechaInicio, [FromQuery] string fechaFin, [FromQuery] string buscar = "")
    {
        try
        {
            var historial = await _ventaService.Lista(fechaInicio, fechaFin, buscar);

            // Mapeamos la lista de entidades a una lista de DTOs para la vista de historial.
            // Nota: Esta vista no necesita los detalles de cada venta, solo la cabecera.
            var listaDto = historial.Select(h => new VentaDTO
            {
                NumeroVenta = h.NumeroVenta,
                NombreUsuario = h.UsuarioRegistrado?.NombreUsuario,
                NombreCliente = h.NombreCliente,
                PrecioTotal = h.precioTotal, // La 'p' es minúscula en tu entidad Venta.cs
                FechaRegistro = DateTime.TryParse(h.FechaRegistro, out var fecha) ? fecha : null,
            }).ToList();

            return Ok(listaDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}