using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _httpClient;
        private readonly ICsrfService _csrfService;

        public ProductoService(HttpClient httpClient, ICsrfService csrfService)
        {
            _httpClient = httpClient;
            _csrfService = csrfService;
        }

        public async Task<List<ProductoDTO>> Listar(string buscar = "")
        {
            var result = await _httpClient.GetFromJsonAsync<List<ProductoDTO>>($"api/productos?buscar={buscar}");
            return result ?? new List<ProductoDTO>();
        }
        public async Task<ProductoDTO> ObtenerPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"api/productos/ObtenerPorCodigo/{codigo}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Si el producto no se encuentra, devolvemos un DTO vacío para evitar errores.
                return new ProductoDTO { IdProducto = 0 };
            }
            return await response.Content.ReadFromJsonAsync<ProductoDTO>();
        }

        public async Task<HttpResponseMessage> Crear(ProductoDTO producto)
        {
            // Obtener token CSRF para la operación
            var csrfToken = await _csrfService.GetCurrentCsrfTokenAsync();
            
            // Crear request con token CSRF
            var request = new HttpRequestMessage(HttpMethod.Post, "api/productos");
            request.Headers.Add("X-CSRF-TOKEN", csrfToken);
            request.Content = JsonContent.Create(producto);
            
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Editar(ProductoDTO producto)
        {
            // Obtener token CSRF para la operación
            var csrfToken = await _csrfService.GetCurrentCsrfTokenAsync();
            
            // Crear request con token CSRF
            var request = new HttpRequestMessage(HttpMethod.Put, "api/productos");
            request.Headers.Add("X-CSRF-TOKEN", csrfToken);
            request.Content = JsonContent.Create(producto);
            
            return await _httpClient.SendAsync(request);
        }
    }
}