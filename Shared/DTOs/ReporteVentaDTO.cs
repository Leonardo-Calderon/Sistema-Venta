namespace Shared.DTOs
{
    public class ReporteVentaDTO
    {
        public string? NumeroVenta { get; set; }
        public string? NombreUsuario { get; set; }
        public DateTime? FechaRegistro { get; set; } // Es mejor usar DateTime para las fechas
        public string? Producto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}