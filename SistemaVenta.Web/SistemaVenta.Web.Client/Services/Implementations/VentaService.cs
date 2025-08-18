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
        private readonly ICsrfService _csrfService;

        public VentaService(HttpClient httpClient, ICsrfService csrfService)
        {
            _httpClient = httpClient;
            _csrfService = csrfService;
        }

        public async Task<VentaDTO> Obtener(string numeroVenta)
        {
            var response = await _httpClient.GetFromJsonAsync<VentaDTO>($"api/Ventas/Obtener/{numeroVenta}");
            return response!;
        }
        public async Task<string> Registrar(VentaDTO venta)
        {
            // Obtener token CSRF para la operación
            var csrfToken = await _csrfService.GetCurrentCsrfTokenAsync();
            
            // Crear request con token CSRF
            var request = new HttpRequestMessage(HttpMethod.Post, "api/Ventas/Registrar");
            request.Headers.Add("X-CSRF-TOKEN", csrfToken);
            request.Content = JsonContent.Create(venta);
            
            var response = await _httpClient.SendAsync(request);
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

        public async Task<List<ReporteVentaDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            var url = $"api/Ventas/Reporte?fechaInicio={fechaInicio}&fechaFin={fechaFin}";
            var result = await _httpClient.GetFromJsonAsync<List<ReporteVentaDTO>>(url);
            return result ?? new List<ReporteVentaDTO>();
        }

        public async Task<byte[]> GenerarPDF(string numeroVenta)
        {
            var response = await _httpClient.GetAsync($"api/Ventas/GenerarPDF/{numeroVenta}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                throw new HttpRequestException($"Error al generar PDF: {response.StatusCode}");
            }
        }
    }

    internal class NumeroVentaResponse
    {
        public string? numeroVenta { get; set; }
    }
}