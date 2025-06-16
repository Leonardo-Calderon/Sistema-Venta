// En: SistemaVenta.API/Controllers/CategoriasController.cs
using Microsoft.AspNetCore.Mvc;
using SVServices.Interfaces;
using Shared.DTOs;
using SVRepository.Entities;
using Microsoft.AspNetCore.Authorization; // Lo añadiremos para proteger el endpoint

[ApiController]
[Route("api/[controller]")]
[Authorize] // ¡Importante! Protegemos todo el controlador
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    // 1. Inyección de Dependencia
    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    // 2. Endpoint para LISTAR todas las categorías
    // GET: api/categorias?buscar=texto
    [HttpGet]
    public async Task<IActionResult> Lista(string buscar = "")
    {
        try
        {
            var listaDeEntidades = await _categoriaService.Lista(buscar);

            if (listaDeEntidades == null || !listaDeEntidades.Any())
            {
                return NotFound(new List<CategoriaDTO>()); // Devuelve una lista vacía si no hay resultados
            }

            // Mapeo de Entidad a DTO
            var listaDeDTOs = listaDeEntidades.Select(c => new CategoriaDTO
            {
                IdCategoria = c.IdCategoria,
                Nombre = c.Nombre,
                IdMedida = c.RefMedida.IdMedida,
                NombreMedida = c.RefMedida.Nombre,
                Activo = c.Activo == 1
            }).ToList();

            return Ok(listaDeDTOs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // 3. Endpoint para CREAR una categoría
    // POST: api/categorias
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CategoriaDTO dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Nombre))
        {
            return BadRequest("El nombre de la categoría es obligatorio.");
        }

        try
        {
            // Mapeo de DTO a Entidad
            var entidad = new Categoria
            {
                Nombre = dto.Nombre,
                RefMedida = new Medida { IdMedida = dto.IdMedida }
            };

            var resultadoSp = await _categoriaService.Crear(entidad);

            if (!string.IsNullOrEmpty(resultadoSp)) // Si el SP devolvió un mensaje de error
            {
                return BadRequest(resultadoSp);
            }

            return Ok("Categoría creada con éxito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // 4. Endpoint para EDITAR una categoría
    // PUT: api/categorias
    [HttpPut]
    public async Task<IActionResult> Editar([FromBody] CategoriaDTO dto)
    {
        if (dto == null || dto.IdCategoria == 0)
        {
            return BadRequest("Datos de categoría inválidos.");
        }

        try
        {
            // Mapeo de DTO a Entidad
            var entidad = new Categoria
            {
                IdCategoria = dto.IdCategoria,
                Nombre = dto.Nombre,
                RefMedida = new Medida { IdMedida = dto.IdMedida },
                Activo = dto.Activo ? 1 : 0
            };

            var resultadoSp = await _categoriaService.Editar(entidad);

            if (!string.IsNullOrEmpty(resultadoSp)) // Si el SP devolvió un mensaje de error
            {
                return BadRequest(resultadoSp);
            }

            return Ok("Categoría actualizada con éxito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}