// En: SistemaVenta.API/Controllers/MedidasController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVServices.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Protegemos el controlador para que solo usuarios logueados puedan acceder
public class MedidasController : ControllerBase
{
    private readonly IMedidaService _medidaService;

    public MedidasController(IMedidaService medidaService)
    {
        _medidaService = medidaService;
    }

    /// <summary>
    /// Endpoint para obtener la lista completa de unidades de medida.
    /// </summary>
    /// <returns>Una lista de medidas en formato DTO.</returns>
    [HttpGet]
    public async Task<IActionResult> Lista()
    {
        try
        {
            var listaDeEntidades = await _medidaService.Lista();

            if (listaDeEntidades == null || !listaDeEntidades.Any())
            {
                // Si no hay medidas, devolvemos un 200 OK con una lista vacía.
                return Ok(new List<MedidaDTO>());
            }

            // Mapeamos la lista de entidades a una lista de DTOs.
            var listaDeDTOs = listaDeEntidades.Select(m => new MedidaDTO
            {
                IdMedida = m.IdMedida,
                Nombre = m.Nombre,
                Abreviatura = m.Abreviatura,
                Equivalente = m.Equivalente,
                Valor = m.Valor
            }).ToList();

            return Ok(listaDeDTOs);
        }
        catch (Exception ex)
        {
            // Si ocurre un error inesperado, devolvemos un error 500.
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}