using System.Collections.Generic;

// Asegúrate de que el namespace coincida con tu proyecto compartido.
namespace Shared.DTOs
{
    public class VentaDTO
    {
        public int IdVenta { get; set; }
        public string? NumeroVenta { get; set; }
        public int IdUsuarioRegistro { get; set; }

        public string? NombreUsuario { get; set; }

        public string? NombreCliente { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal PagoCon { get; set; }
        public decimal Cambio { get; set; }
        public string? FechaRegistro { get; set; }
        public virtual ICollection<DetalleVentaDTO> DetalleVenta { get; set; } = new List<DetalleVentaDTO>();
    }
}