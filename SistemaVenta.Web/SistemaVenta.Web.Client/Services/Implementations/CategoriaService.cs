using Shared.DTOs;
using System.Net.Http.Json;
using SistemaVenta.Web.Client.Services.Interfaces;

namespace SistemaVenta.Web.Client.Services.Implementations;

public class CategoriaService : ICategoriaService
{
    private readonly HttpClient _httpClient;
    public CategoriaService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<List<CategoriaDTO>> Lista(string buscar)
    {
        return await _httpClient.GetFromJsonAsync<List<CategoriaDTO>>($"api/categorias?buscar={buscar}");
    }

    public async Task<HttpResponseMessage> Crear(CategoriaDTO categoria)
    {
        return await _httpClient.PostAsJsonAsync("api/categorias", categoria);
    }

    public async Task<HttpResponseMessage> Editar(CategoriaDTO categoria)
    {
        return await _httpClient.PutAsJsonAsync($"api/categorias/{categoria.IdCategoria}", categoria);
    }

}