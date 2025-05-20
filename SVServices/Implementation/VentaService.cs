
using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    public class VentaService : IVentaService
    {

        private readonly IVentaRepository _ventaRepository;
        public VentaService(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        async Task<string> IVentaService.Registrar(string ventaXML)
        {
            return await _ventaRepository.Registrar(ventaXML);
        }

        async Task<Venta> IVentaService.Obtener(string numeroVenta)
        {
            return await _ventaRepository.Obtener(numeroVenta);
        }

        async Task<List<DetalleVenta>> IVentaService.ObtenerDetalle(string numeroVenta)
        {
            return await _ventaRepository.ObtenerDetalle(numeroVenta);
        }

        public async Task<List<Venta>> Lista(string fechaInicio, string fechaFin, string buscar = "")
        {
            return await _ventaRepository.Lista(fechaInicio, fechaFin, buscar);
        }

        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            return await _ventaRepository.Reporte(fechaInicio, fechaFin);
        }
    }
}
