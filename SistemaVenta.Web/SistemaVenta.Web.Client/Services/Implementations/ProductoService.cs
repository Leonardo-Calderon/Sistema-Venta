using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductoDTO>> Listar(string buscar = "")
        {
            var result = await _httpClient.GetFromJsonAsync<List<ProductoDTO>>($"api/productos?buscar={buscar}");
            return result ?? new List<ProductoDTO>();
        }

        // --- IMPLEMENTACIÓN AÑADIDA ---
        public async Task<ProductoDTO> ObtenerPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"api/productos/ObtenerPorCodigo/{codigo}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Si el producto no se encuentra, devolvemos un DTO vacío para evitar errores.
                return new ProductoDTO { IdProducto = 0 };
            }

            // Si se encuentra, lo deserializamos y lo devolvemos.
            return await response.Content.ReadFromJsonAsync<ProductoDTO>();
        }

        public async Task<HttpResponseMessage> Crear(ProductoDTO producto)
        {
            return await _httpClient.PostAsJsonAsync("api/productos", producto);
        }

        public async Task<HttpResponseMessage> Editar(ProductoDTO producto)
        {
            return await _httpClient.PutAsJsonAsync("api/productos", producto);
        }
    }
}