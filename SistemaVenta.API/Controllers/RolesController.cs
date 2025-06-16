// En: SistemaVenta.API/Controllers/RolesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVServices.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRolService _rolService;

    public RolesController(IRolService rolService)
    {
        _rolService = rolService;
    }

    /// <summary>
    /// Devuelve la lista de todos los roles.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Lista()
    {
        try
        {
            var listaDeEntidades = await _rolService.Lista();

            if (listaDeEntidades == null || !listaDeEntidades.Any())
            {
                return Ok(new List<RolDTO>());
            }

            // Mapeamos la lista de entidades a una lista de DTOs.
            var listaDeDTOs = listaDeEntidades.Select(r => new RolDTO
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre
            }).ToList();

            return Ok(listaDeDTOs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}