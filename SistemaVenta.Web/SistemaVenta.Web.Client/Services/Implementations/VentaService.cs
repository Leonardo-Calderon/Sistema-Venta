using Shared.DTOs;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    public class VentaService : IVentaService
    {
        private readonly HttpClient _httpClient;

        public VentaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<VentaDTO> Obtener(string numeroVenta)
        {
            var response = await _httpClient.GetFromJsonAsync<VentaDTO>($"api/Ventas/Obtener/{numeroVenta}");
            return response!;
        }
        public async Task<string> Registrar(VentaDTO venta)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Ventas/Registrar", venta);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<NumeroVentaResponse>();
                return responseBody?.numeroVenta ?? string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<List<VentaDTO>> Historial(string fechaInicio, string fechaFin, string buscar)
        {
            var response = await _httpClient.GetFromJsonAsync<List<VentaDTO>>($"api/Ventas/Historial?fechaInicio={fechaInicio}&fechaFin={fechaFin}&buscar={buscar}");
            return response ?? new List<VentaDTO>();
        }

        public async Task<List<DetalleVentaDTO>> Detalle(string numeroVenta)
        {
            var response = await _httpClient.GetFromJsonAsync<List<DetalleVentaDTO>>($"api/Ventas/Detalle/{numeroVenta}");
            return response ?? new List<DetalleVentaDTO>();
        }

        public async Task<List<DetalleVentaDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            var response = await _httpClient.GetFromJsonAsync<List<DetalleVentaDTO>>($"api/Ventas/Reporte?fechaInicio={fechaInicio}&fechaFin={fechaFin}");
            return response ?? new List<DetalleVentaDTO>();
        }
    }

    internal class NumeroVentaResponse
    {
        public string? numeroVenta { get; set; }
    }
}