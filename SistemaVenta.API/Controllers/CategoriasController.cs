using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVRepository.Entities;
using SVServices.Interfaces;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de categorías de productos.
/// </summary>
/// <remarks>
/// Este controlador proporciona endpoints para:
/// - Obtener lista de categorías con búsqueda opcional
/// - Crear nuevas categorías (solo administradores)
/// - Editar categorías existentes (solo administradores)
/// - Eliminar categorías (solo administradores)
/// - Obtener una categoría específica por ID
/// 
/// Implementa control de acceso basado en roles:
/// - Lectura: Usuarios autenticados
/// - Escritura: Solo administradores
/// 
/// Todas las operaciones están protegidas por autenticación JWT.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Protege todo el controlador, requiriendo que el usuario esté logueado.
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;
    private readonly ILogger<CategoriasController> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de categorías.
    /// </summary>
    /// <param name="categoriaService">Servicio para operaciones de categorías.</param>
    /// <param name="logger">Logger para registrar información de operaciones.</param>
    /// <remarks>
    /// El constructor recibe las dependencias necesarias para el funcionamiento
    /// del controlador, incluyendo el servicio de categorías y logging.
    /// </remarks>
    public CategoriasController(ICategoriaService categoriaService, ILogger<CategoriasController> logger)
    {
        _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene una lista de todas las categorías, con opción de búsqueda.
    /// </summary>
    /// <param name="buscar">Término de búsqueda opcional para filtrar categorías por nombre.</param>
    /// <returns>
    /// - 200 OK con la lista de categorías si la operación es exitosa
    /// - 500 Internal Server Error si ocurre un error interno
    /// </returns>
    /// <remarks>
    /// Este endpoint permite obtener todas las categorías disponibles en el sistema.
    /// Si se proporciona un término de búsqueda, filtra las categorías cuyo nombre
    /// contenga el término especificado (búsqueda no sensible a mayúsculas/minúsculas).
    /// 
    /// La respuesta incluye información completa de cada categoría:
    /// - ID de la categoría
    /// - Nombre de la categoría
    /// - ID y nombre de la medida asociada
    /// - Estado activo/inactivo
    /// 
    /// Accesible para cualquier usuario autenticado.
    /// </remarks>
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

    /// <summary>
    /// Crea una nueva categoría en el sistema.
    /// </summary>
    /// <param name="dto">Datos de la categoría a crear.</param>
    /// <returns>
    /// - 200 OK si la categoría se crea exitosamente
    /// - 400 Bad Request si los datos son inválidos o hay errores de validación
    /// - 401 Unauthorized si el usuario no tiene permisos de administrador
    /// - 500 Internal Server Error si ocurre un error interno
    /// </returns>
    /// <remarks>
    /// Este endpoint permite crear una nueva categoría en el sistema.
    /// La categoría se crea con estado activo por defecto.
    /// 
    /// Validaciones realizadas:
    /// - ModelState validation
    /// - Validación de datos en el servicio de categorías
    /// 
    /// Requiere rol de Administrador para acceder.
    /// </remarks>
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

    /// <summary>
    /// Edita una categoría existente en el sistema.
    /// </summary>
    /// <param name="id">ID de la categoría a editar.</param>
    /// <param name="dto">Datos actualizados de la categoría.</param>
    /// <returns>
    /// - 200 OK si la categoría se edita exitosamente
    /// - 400 Bad Request si los datos son inválidos o hay errores de validación
    /// - 401 Unauthorized si el usuario no tiene permisos de administrador
    /// - 500 Internal Server Error si ocurre un error interno
    /// </returns>
    /// <remarks>
    /// Este endpoint permite editar una categoría existente en el sistema.
    /// 
    /// Validaciones realizadas:
    /// - Verificación de que el ID en la URL coincida con el ID en el DTO
    /// - ModelState validation
    /// - Validación de datos en el servicio de categorías
    /// 
    /// Requiere rol de Administrador para acceder.
    /// </remarks>
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