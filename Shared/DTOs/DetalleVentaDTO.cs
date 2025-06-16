
namespace Shared.DTOs
{
    public class DetalleVentaDTO
    {
        // Al crear una venta, solo necesitamos el Id del producto.
        public int IdProducto { get; set; }

        // Estos campos son útiles para mostrar la información en la UI.
        public string? DescripcionProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; } // Corresponde al PrecioVenta de la entidad
        public decimal Total { get; set; }  // Corresponde al PrecioTotal de la entidad
    }
}