using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVRepository.Entities;
using SVServices.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SistemaVenta.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NegocioController : ControllerBase
    {
        private readonly INegocioService _negocioService;
        private readonly ICloudinaryService _cloudinaryService;

        // Inyectamos ambos servicios, como en la app de escritorio
        public NegocioController(INegocioService negocioService, ICloudinaryService cloudinaryService)
        {
            _negocioService = negocioService;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")] // Solo administradores pueden ver configuración del negocio
        public async Task<IActionResult> Obtener()
        {
            try
            {
                var entidad = await _negocioService.Obtener();
                if (entidad == null) return NotFound();

                // El mapeo a DTO es correcto
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

        // Usamos HttpPost y FromForm para poder recibir datos y un archivo.
        [HttpPost("GuardarCambios")]
        [Authorize(Roles = "Administrador")] // Solo administradores pueden modificar configuración del negocio
        public async Task<IActionResult> GuardarCambios([FromForm] NegocioDTO dto, IFormFile? logo)
        {
            if (dto == null)
            {
                return BadRequest("Los datos del negocio no pueden ser nulos.");
            }

            try
            {
                // Obtenemos el negocio actual para saber si hay un logo antiguo que borrar
                var negocioActual = await _negocioService.Obtener();

                // Si se ha enviado un nuevo logo desde Blazor
                if (logo != null)
                {
                    using var stream = logo.OpenReadStream();
                    var cloudinaryResponse = await _cloudinaryService.SubirImagen(logo.FileName, stream);

                    if (!string.IsNullOrEmpty(cloudinaryResponse.PublicId))
                    {
                        // Si el negocio ya tenía un logo, borramos el antiguo de Cloudinary
                        if (!string.IsNullOrEmpty(negocioActual.NombreLogo))
                        {
                            await _cloudinaryService.EliminarImagen(negocioActual.NombreLogo);
                        }
                        // Actualizamos el DTO con los nuevos datos del logo
                        dto.NombreLogo = cloudinaryResponse.PublicId;
                        dto.URL = cloudinaryResponse.SecureUrl;
                    }
                }
                else // Si no se envió un nuevo logo, mantenemos el existente
                {
                    dto.NombreLogo = negocioActual.NombreLogo;
                    dto.URL = negocioActual.URL;
                }

                // Mapeamos el DTO (ya actualizado con el logo si corresponde) a la entidad
                var entidad = new Negocio
                {
                    IdNegocio = negocioActual.IdNegocio, // Siempre es el Id 1
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

                // Devolvemos el DTO actualizado para que el cliente refresque sus datos
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}