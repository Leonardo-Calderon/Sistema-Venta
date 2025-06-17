using Shared.DTOs;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    public class RolService : IRolService
    {
        private readonly HttpClient _httpClient;

        public RolService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RolDTO>> Lista()
        {
            return await _httpClient.GetFromJsonAsync<List<RolDTO>>("api/roles");
        }
    }
}