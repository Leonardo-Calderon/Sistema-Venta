namespace Shared.DTOs
{
    public class DetalleVentaDTO
    {
        public int IdDetalleVenta { get; set; }
        public int IdProducto { get; set; }
        public string? DescripcionProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
    }
}