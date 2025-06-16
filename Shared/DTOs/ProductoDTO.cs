namespace Shared.DTOs
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public string? DescripcionCategoria { get; set; } // Nombre de la categoría para mostrar
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; } // Representa el stock
        public bool Activo { get; set; }
    }
}