// En: SistemaVenta.API/Controllers/ProductosController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVRepository.Entities;
using SVServices.Interfaces;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    // GET: api/productos?buscar=texto
    [HttpGet]
    public async Task<IActionResult> Lista(string buscar = "")
    {
        try
        {
            var listaEntidades = await _productoService.Lista(buscar);

            if (listaEntidades == null || !listaEntidades.Any())
                return Ok(new List<ProductoDTO>());

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
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // GET: api/productos/codigo/{codigo}
    [HttpGet("codigo/{codigo}")]
    public async Task<IActionResult> ObtenerPorCodigo(string codigo)
    {
        try
        {
            var entidad = await _productoService.Obtener(codigo);
            if (entidad == null || entidad.IdProducto == 0)
            {
                return NotFound($"No se encontró un producto con el código '{codigo}'.");
            }

            var dto = new ProductoDTO
            {
                IdProducto = entidad.IdProducto,
                Codigo = entidad.Codigo,
                Descripcion = entidad.Descripcion,
                // Nota: El SP sp_obtenerProducto debe devolver estos campos para que no sean nulos
                IdCategoria = entidad.RefCategoria?.IdCategoria ?? 0,
                DescripcionCategoria = entidad.RefCategoria?.Nombre,
                PrecioCompra = entidad.PrecioCompra,
                PrecioVenta = entidad.PrecioVenta,
                Cantidad = entidad.Cantidad,
                Activo = entidad.Activo == 1
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    // POST: api/productos
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] ProductoDTO dto)
    {
        if (dto == null) return BadRequest("Datos de producto inválidos.");

        try
        {
            var entidad = new Producto
            {
                Codigo = dto.Codigo,
                Descripcion = dto.Descripcion,
                PrecioCompra = dto.PrecioCompra,
                PrecioVenta = dto.PrecioVenta,
                Cantidad = dto.Cantidad,
                RefCategoria = new Categoria { IdCategoria = dto.IdCategoria }
            };

            var resultadoSp = await _productoService.Crear(entidad);
            if (!string.IsNullOrEmpty(resultadoSp))
            {
                return BadRequest(resultadoSp);
            }

            return Ok("Producto creado con éxito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // PUT: api/productos
    [HttpPut]
    public async Task<IActionResult> Editar([FromBody] ProductoDTO dto)
    {
        if (dto == null || dto.IdProducto == 0) return BadRequest("Datos de producto inválidos.");

        try
        {
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

            return Ok("Producto actualizado con éxito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // DELETE: api/productos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        // NOTA IMPORTANTE: Este método no existe en tu repositorio/servicio.
        // Debes añadirlo para que este endpoint funcione.
        try
        {
            // var resultado = await _productoService.Eliminar(id);
            // if(!resultado) return BadRequest("No se pudo eliminar el producto.");

            // return Ok("Producto eliminado con éxito.");

            // Por ahora, devolvemos un "No Implementado"
            return StatusCode(501, "La funcionalidad de eliminar no está implementada en el servicio.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}