// En: SistemaVenta.API/Controllers/NegocioController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVRepository.Entities;
using SVServices.Interfaces;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Protegemos el controlador
public class NegocioController : ControllerBase
{
    private readonly INegocioService _negocioService;

    public NegocioController(INegocioService negocioService)
    {
        _negocioService = negocioService;
    }

    /// <summary>
    /// Obtiene los datos del negocio.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Obtener()
    {
        try
        {
            var entidad = await _negocioService.Obtener();
            if (entidad == null)
            {
                return NotFound(); // Opcional: si la tabla pudiera estar vacía
            }

            // Mapeamos la entidad al DTO que se enviará al cliente
            var dto = new NegocioDTO
            {
                IdNegocio = entidad.IdNegocio,
                RazonSocial = entidad.RazonSocial,
                RFC = entidad.RFC,
                Direccion = entidad.Direccion,
                Celular = entidad.Celular,
                Correo = entidad.Correo,
                SimboloMoneda = entidad.SimboloMoneda,
                NombreLogo = entidad.NombreLogo,
                URL = entidad.URL
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    /// <summary>
    /// Guarda o actualiza los datos del negocio.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Editar([FromBody] NegocioDTO dto)
    {
        if (dto == null)
        {
            return BadRequest("Los datos del negocio no pueden ser nulos.");
        }

        try
        {
            // Mapeamos el DTO a la entidad que recibirá el servicio
            var entidad = new Negocio
            {
                IdNegocio = dto.IdNegocio,
                RazonSocial = dto.RazonSocial,
                RFC = dto.RFC,
                Direccion = dto.Direccion,
                Celular = dto.Celular,
                Correo = dto.Correo,
                SimboloMoneda = dto.SimboloMoneda,
                NombreLogo = dto.NombreLogo,
                URL = dto.URL
            };

            await _negocioService.Editar(entidad);

            return Ok("Datos del negocio actualizados con éxito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}