using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs; 
using SVRepository.Entities;
using SVServices.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Autorización a nivel de controlador
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Lista(string buscar = "")
    {
        var listaEntidades = await _productoService.Lista(buscar);

        // El mapeo aquí es correcto.
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

    // --- NUEVO ENDPOINT AÑADIDO ---
    // Este endpoint es crucial y ya existía la lógica en tu repositorio.
    // Permite buscar un producto para, por ejemplo, agregarlo a una venta.
    [HttpGet("ObtenerPorCodigo/{codigo}")]
    [AllowAnonymous] // Se permite el acceso anónimo para la búsqueda de productos en la venta
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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Mapeo corregido para incluir todos los campos necesarios.
        var entidad = new Producto
        {
            Codigo = dto.Codigo,
            Descripcion = dto.Descripcion,
            PrecioCompra = dto.PrecioCompra,
            PrecioVenta = dto.PrecioVenta,
            Cantidad = dto.Cantidad,
            Activo = dto.Activo ? 1 : 0, // Se añade la conversión de bool a int
            RefCategoria = new Categoria { IdCategoria = dto.IdCategoria }
        };

        var resultadoSp = await _productoService.Crear(entidad);
        if (!string.IsNullOrEmpty(resultadoSp))
        {
            return BadRequest(resultadoSp);
        }

        return Ok();
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