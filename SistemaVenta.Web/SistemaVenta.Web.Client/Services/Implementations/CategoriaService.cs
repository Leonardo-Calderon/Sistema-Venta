using Shared.DTOs;
using System.Net.Http.Json;
using SistemaVenta.Web.Client.Services.Interfaces;

namespace SistemaVenta.Web.Client.Services.Implementations;

public class CategoriaService : ICategoriaService
{
    private readonly HttpClient _httpClient;
    private readonly ICsrfService _csrfService;
    
    public CategoriaService(HttpClient httpClient, ICsrfService csrfService) 
    { 
        _httpClient = httpClient; 
        _csrfService = csrfService;
    }

    public async Task<List<CategoriaDTO>> Lista(string buscar)
    {
        return await _httpClient.GetFromJsonAsync<List<CategoriaDTO>>($"api/categorias?buscar={buscar}");
    }

    public async Task<HttpResponseMessage> Crear(CategoriaDTO categoria)
    {
        // Obtener token CSRF para la operación
        var csrfToken = await _csrfService.GetCurrentCsrfTokenAsync();
        
        // Crear request con token CSRF
        var request = new HttpRequestMessage(HttpMethod.Post, "api/categorias");
        request.Headers.Add("X-CSRF-TOKEN", csrfToken);
        request.Content = JsonContent.Create(categoria);
        
        return await _httpClient.SendAsync(request);
    }

    public async Task<HttpResponseMessage> Editar(CategoriaDTO categoria)
    {
        // Obtener token CSRF para la operación
        var csrfToken = await _csrfService.GetCurrentCsrfTokenAsync();
        
        // Crear request con token CSRF
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/categorias/{categoria.IdCategoria}");
        request.Headers.Add("X-CSRF-TOKEN", csrfToken);
        request.Content = JsonContent.Create(categoria);
        
        return await _httpClient.SendAsync(request);
    }

}