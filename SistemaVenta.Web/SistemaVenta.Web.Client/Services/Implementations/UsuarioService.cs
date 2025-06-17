using Shared.DTOs;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _httpClient;

        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UsuarioDTO>> Lista(string buscar)
        {
            return await _httpClient.GetFromJsonAsync<List<UsuarioDTO>>($"api/usuarios?buscar={buscar}");
        }

        public async Task<HttpResponseMessage> Crear(UsuarioCrearDTO usuario)
        {
            return await _httpClient.PostAsJsonAsync("api/usuarios", usuario);
        }

        public async Task<HttpResponseMessage> Editar(UsuarioDTO usuario)
        {
            return await _httpClient.PutAsJsonAsync($"api/usuarios/{usuario.IdUsuario}", usuario);
        }

        public async Task<HttpResponseMessage> Eliminar(int id)
        {
            return await _httpClient.DeleteAsync($"api/usuarios/{id}");
        }
    }
}