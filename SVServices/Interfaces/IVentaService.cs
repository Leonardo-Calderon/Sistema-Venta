
using SVRepository.Entities;

namespace SVServices.Interfaces
{
    public interface IVentaService
    {
        Task<string> Registrar(string ventaXML);
        Task<Venta> Obtener(string numeroVenta);
        Task<List<DetalleVenta>> ObtenerDetalle(string numeroVenta);
        Task<List<Venta>> Lista(string fechaInicio, string fechaFin, string buscar = "");
        Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin);
    }
}
