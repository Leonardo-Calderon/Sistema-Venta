using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs; 
using SVRepository.Entities;
using SVServices.Interfaces;
using Microsoft.Extensions.Logging;

/// <summary>
/// Controlador para gestionar las operaciones CRUD de productos en el sistema.
/// </summary>
/// <remarks>
/// Este controlador proporciona endpoints para:
/// - Obtener lista de productos con búsqueda opcional
/// - Búsqueda segura de productos con validación y sanitización
/// - Crear nuevos productos (solo administradores)
/// - Editar productos existentes (solo administradores)
/// - Eliminar productos (solo administradores)
/// - Obtener un producto específico por ID
/// - Actualizar stock de productos
/// 
/// Implementa medidas de seguridad:
/// - Control de acceso basado en roles
/// - Validación y sanitización de entrada de datos
/// - Prevención de inyección SQL
/// - Logging de actividades de seguridad
/// 
/// Roles requeridos:
/// - Lectura: Administrador, Ventas
/// - Escritura: Solo Administrador
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Autorización a nivel de controlador
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly IValidacionService _validacionService;
    private readonly ILogger<ProductosController> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de productos.
    /// </summary>
    /// <param name="productoService">Servicio para operaciones de productos.</param>
    /// <param name="validacionService">Servicio para validación y sanitización de datos.</param>
    /// <param name="logger">Logger para registrar información de operaciones.</param>
    /// <remarks>
    /// El constructor recibe las dependencias necesarias para el funcionamiento
    /// del controlador, incluyendo servicios de productos, validación y logging.
    /// </remarks>
    public ProductosController(IProductoService productoService, IValidacionService validacionService, ILogger<ProductosController> logger)
    {
        _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
        _validacionService = validacionService ?? throw new ArgumentNullException(nameof(validacionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene una lista de todos los productos, con opción de búsqueda.
    /// </summary>
    /// <param name="buscar">Término de búsqueda opcional para filtrar productos.</param>
    /// <returns>
    /// - 200 OK con la lista de productos si la operación es exitosa
    /// - 401 Unauthorized si el usuario no tiene permisos
    /// - 500 Internal Server Error si ocurre un error interno
    /// </returns>
    /// <remarks>
    /// Este endpoint permite obtener todos los productos disponibles en el sistema.
    /// Si se proporciona un término de búsqueda, filtra los productos que coincidan
    /// con el término especificado.
    /// 
    /// La respuesta incluye información completa de cada producto:
    /// - ID y código del producto
    /// - Descripción del producto
    /// - Categoría asociada
    /// - Precios de compra y venta
    /// - Cantidad en stock
    /// - Estado activo/inactivo
    /// 
    /// Requiere roles: Administrador o Ventas.
    /// </remarks>
    [HttpGet]
    [Authorize(Roles = "Administrador,Ventas")]
    public async Task<IActionResult> Lista(string buscar = "")
    {
        var listaEntidades = await _productoService.Lista(buscar);

        // Mapeo de entidades a DTOs
        var listaDto = listaEntidades.Select(p => new ProductoDTO
        {
            IdProducto = p.IdProducto,
            Codigo = p.Codigo,
            Descripcion = p.Descripcion,
            IdCategoria = p.RefCategoria.IdCategoria,
            DescripcionCategoria = p.RefCategoria.Nombre,
            PrecioCompra = p.PrecioCompra,
            PrecioVenta = p.PrecioVenta,
            Cantidad = p.Cantidad,
            Activo = p.Activo == 1
        }).ToList();

        return Ok(listaDto);
    }

    /// <summary>
    /// Realiza una búsqueda segura de productos con validación y sanitización de entrada.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda para filtrar productos.</param>
    /// <returns>
    /// - 200 OK con la lista de productos filtrados si la operación es exitosa
    /// - 400 Bad Request si el término de búsqueda es inválido o contiene caracteres peligrosos
    /// - 401 Unauthorized si el usuario no tiene permisos
    /// - 500 Internal Server Error si ocurre un error interno
    /// </returns>
    /// <remarks>
    /// Este endpoint implementa búsqueda segura con múltiples capas de validación:
    /// 
    /// Validaciones de seguridad:
    /// - Sanitización del término de búsqueda
    /// - Detección de caracteres peligrosos
    /// - Validación de longitud mínima (2 caracteres) y máxima (50 caracteres)
    /// - Prevención de inyección SQL
    /// 
    /// Si no se proporciona término de búsqueda, retorna la lista completa de productos.
    /// 
    /// Requiere roles: Administrador o Ventas.
    /// </remarks>
    [HttpGet("search")]
    [Authorize(Roles = "Administrador,Ventas")]
    public async Task<IActionResult> BusquedaSegura([FromQuery] string searchTerm = "")
    {
        try
        {
            // Validación y sanitización del término de búsqueda
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogInformation("Búsqueda de productos sin término de búsqueda");
                return await Lista(""); // Retornar lista completa
            }

            // Sanitizar el término de búsqueda
            var searchTermSanitizado = _validacionService.SanitizarString(searchTerm, 50, true);
            
            // Verificar si el término de búsqueda fue rechazado por la sanitización
            if (searchTermSanitizado == null)
            {
                _logger.LogWarning("Término de búsqueda rechazado por sanitización: {SearchTerm}", searchTerm);
                return BadRequest("El término de búsqueda contiene caracteres no permitidos.");
            }

            // Verificar si contiene caracteres peligrosos
            if (_validacionService.ContieneCaracteresPeligrosos(searchTerm))
            {
                _logger.LogWarning("Se detectaron caracteres peligrosos en la búsqueda de productos: {SearchTerm}", searchTerm);
                return BadRequest("El término de búsqueda contiene caracteres no permitidos.");
            }

            // Validar longitud mínima y máxima
            if (searchTermSanitizado.Length < 2)
            {
                _logger.LogWarning("Término de búsqueda de productos demasiado corto: {SearchTerm}", searchTermSanitizado);
                return BadRequest("El término de búsqueda debe tener al menos 2 caracteres.");
            }

            if (searchTermSanitizado.Length > 50)
            {
                _logger.LogWarning("Término de búsqueda de productos demasiado largo: {SearchTerm}", searchTermSanitizado);
                return BadRequest("El término de búsqueda no puede exceder 50 caracteres.");
            }

            // Realizar búsqueda segura usando el servicio
            _logger.LogInformation("Iniciando búsqueda segura de productos con término: {SearchTerm}", searchTermSanitizado);
            
            var listaEntidades = await _productoService.Lista(searchTermSanitizado);
            
            // Mapear a DTOs
            var listaDto = listaEntidades.Select(p => new ProductoDTO
            {
                IdProducto = p.IdProducto,
                Codigo = p.Codigo,
                Descripcion = p.Descripcion,
                IdCategoria = p.RefCategoria.IdCategoria,
                DescripcionCategoria = p.RefCategoria.Nombre,
                PrecioCompra = p.PrecioCompra,
                PrecioVenta = p.PrecioVenta,
                Cantidad = p.Cantidad,
                Activo = p.Activo == 1
            }).ToList();

            _logger.LogInformation("Búsqueda segura de productos completada. Resultados encontrados: {Count}", listaDto.Count);
            
            return Ok(new
            {
                TerminoBusqueda = searchTermSanitizado,
                TotalResultados = listaDto.Count,
                Resultados = listaDto,
                FechaBusqueda = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en búsqueda segura de productos con término: {SearchTerm}", searchTerm);
            return StatusCode(500, "Error interno del servidor durante la búsqueda.");
        }
    }

    // --- NUEVO ENDPOINT AÑADIDO ---
    // Este endpoint es crucial y ya existía la lógica en tu repositorio.
    // Permite buscar un producto para, por ejemplo, agregarlo a una venta.
    [HttpGet("ObtenerPorCodigo/{codigo}")]
    [Authorize] // Requiere autenticación para buscar productos
    public async Task<IActionResult> ObtenerPorCodigo(string codigo)
    {
        var p = await _productoService.Obtener(codigo);
        if (p.IdProducto == 0)
        {
            return NotFound("Producto no encontrado.");
        }

        var dto = new ProductoDTO
        {
            IdProducto = p.IdProducto,
            Codigo = p.Codigo,
            Descripcion = p.Descripcion,
            IdCategoria = p.RefCategoria.IdCategoria,
            DescripcionCategoria = p.RefCategoria.Nombre,
            PrecioCompra = p.PrecioCompra,
            PrecioVenta = p.PrecioVenta,
            Cantidad = p.Cantidad,
            Activo = p.Activo == 1
        };

        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] ProductoDTO dto)
    {
        try
        {
            _logger.LogInformation("Iniciando creación de producto: {Codigo}", dto.Codigo);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido al crear producto: {Errors}", 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            // Validar datos antes de crear
            if (string.IsNullOrWhiteSpace(dto.Codigo))
            {
                _logger.LogWarning("Código de producto vacío");
                return BadRequest("El código del producto es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(dto.Descripcion))
            {
                _logger.LogWarning("Descripción de producto vacía");
                return BadRequest("La descripción del producto es obligatoria");
            }

            if (dto.IdCategoria <= 0)
            {
                _logger.LogWarning("Categoría inválida: {IdCategoria}", dto.IdCategoria);
                return BadRequest("Debe seleccionar una categoría válida");
            }

            // Mapeo corregido para incluir todos los campos necesarios.
            var entidad = new Producto
            {
                Codigo = dto.Codigo?.Trim(),
                Descripcion = dto.Descripcion?.Trim(),
                PrecioCompra = dto.PrecioCompra,
                PrecioVenta = dto.PrecioVenta,
                Cantidad = dto.Cantidad,
                Activo = dto.Activo ? 1 : 0, 
                RefCategoria = new Categoria { IdCategoria = dto.IdCategoria }
            };

            _logger.LogInformation("Entidad producto creada, llamando al servicio: {Codigo}, {Descripcion}, {IdCategoria}", 
                entidad.Codigo, entidad.Descripcion, entidad.RefCategoria.IdCategoria);

            var resultadoSp = await _productoService.Crear(entidad);
            
            if (!string.IsNullOrEmpty(resultadoSp))
            {
                _logger.LogWarning("Error al crear producto: {Error}", resultadoSp);
                return BadRequest(resultadoSp);
            }

            _logger.LogInformation("Producto creado exitosamente: {Codigo}", dto.Codigo);
            return Ok(new { message = "Producto creado exitosamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al crear producto: {Codigo}", dto.Codigo);
            return StatusCode(500, "Error interno del servidor al crear el producto");
        }
    }

    [HttpPut]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar([FromBody] ProductoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // El mapeo aquí ya era correcto y se mantiene.
        var entidad = new Producto
        {
            IdProducto = dto.IdProducto,
            Codigo = dto.Codigo,
            Descripcion = dto.Descripcion,
            PrecioCompra = dto.PrecioCompra,
            PrecioVenta = dto.PrecioVenta,
            Cantidad = dto.Cantidad,
            Activo = dto.Activo ? 1 : 0,
            RefCategoria = new Categoria { IdCategoria = dto.IdCategoria }
        };

        var resultadoSp = await _productoService.Editar(entidad);
        if (!string.IsNullOrEmpty(resultadoSp))
        {
            return BadRequest(resultadoSp);
        }

        return Ok();
    }
}