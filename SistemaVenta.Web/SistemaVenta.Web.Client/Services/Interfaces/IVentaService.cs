using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    public interface IVentaService
    {
        Task<string> Registrar(VentaDTO venta);
        Task<List<VentaDTO>> Historial(string fechaInicio, string fechaFin, string buscar);
        Task<VentaDTO> Obtener(string numeroVenta);
        Task<List<DetalleVentaDTO>> Detalle(string numeroVenta);
        Task<List<DetalleVentaDTO>> Reporte(string fechaInicio, string fechaFin);
    }
}