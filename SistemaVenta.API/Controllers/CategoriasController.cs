using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVRepository.Entities;
using SVServices.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Protege todo el controlador, requiriendo que el usuario esté logueado.
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ICategoriaService categoriaService, ILogger<CategoriasController> logger)
    {
        _categoriaService = categoriaService;
        _logger = logger;
    }

    // GET: api/categorias?buscar=
    /// <summary>
    /// Obtiene una lista de todas las categorías, con opción de búsqueda.
    /// Accesible para cualquier usuario autenticado.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Lista(string buscar = "")
    {
        try
        {
            var listaDeEntidades = await _categoriaService.Lista(buscar);

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
            _logger.LogError(ex, "Error al obtener la lista de categorías.");
            return StatusCode(500, "Error interno del servidor al procesar la solicitud.");
        }
    }

    // POST: api/categorias
    /// <summary>
    /// Crea una nueva categoría.
    /// Requiere rol de Administrador.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] CategoriaDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var entidad = new Categoria
            {
                Nombre = dto.Nombre,
                RefMedida = new Medida { IdMedida = dto.IdMedida },
                Activo = 1 // Las categorías nuevas se crean activas por defecto
            };

            var resultadoSp = await _categoriaService.Crear(entidad);

            if (!string.IsNullOrEmpty(resultadoSp))
            {
                return BadRequest(resultadoSp);
            }

            return Ok("Categoría creada con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la categoría.");
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    // PUT: api/categorias/5
    /// <summary>
    /// Edita una categoría existente.
    /// Requiere rol de Administrador.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id, [FromBody] CategoriaDTO dto)
    {
        if (id != dto.IdCategoria)
        {
            return BadRequest("El ID de la ruta no coincide con el ID del objeto.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var entidad = new Categoria
            {
                IdCategoria = dto.IdCategoria,
                Nombre = dto.Nombre,
                RefMedida = new Medida { IdMedida = dto.IdMedida },
                Activo = dto.Activo ? 1 : 0
            };

            var resultadoSp = await _categoriaService.Editar(entidad);

            if (!string.IsNullOrEmpty(resultadoSp))
            {
                return BadRequest(resultadoSp);
            }

            return Ok("Categoría actualizada con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar la categoría con ID {CategoriaId}.", id);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

}