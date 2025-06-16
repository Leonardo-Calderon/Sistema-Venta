namespace Shared.DTOs
{
    public class VentaDTO
    {
        public int IdVenta { get; set; }
        public string? NumeroVenta { get; set; }
        public int? IdUsuario { get; set; } // Para saber quién registra la venta
        public string? NombreUsuario { get; set; } // Para mostrar en el historial
        public string? NombreCliente { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal PagoCon { get; set; }
        public decimal Cambio { get; set; }
        public DateTime? FechaRegistro { get; set; }

        // La propiedad más importante: la lista de detalles
        public List<DetalleVentaDTO> Detalles { get; set; } = new List<DetalleVentaDTO>();
    }
}