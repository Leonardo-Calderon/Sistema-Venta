using Shared.DTOs;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    public class NegocioService : INegocioService
    {
        private readonly HttpClient _httpClient;
        public NegocioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<NegocioDTO> Obtener()
        {
            return await _httpClient.GetFromJsonAsync<NegocioDTO>("api/Negocio");
        }

        public async Task<NegocioDTO> GuardarCambios(MultipartFormDataContent model)
        {
            var response = await _httpClient.PostAsync("api/Negocio/GuardarCambios", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<NegocioDTO>();
            }
            else
            {
                Console.WriteLine("Error al guardar los cambios del negocio");
                return null;
            }
        }
    }
}