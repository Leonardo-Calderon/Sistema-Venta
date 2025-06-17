using Shared.DTOs;
using System.Net.Http.Json;
using SistemaVenta.Web.Client.Services.Interfaces;

namespace SistemaVenta.Web.Client.Services.Implementations;

public class MedidaService : IMedidaService
{
    private readonly HttpClient _httpClient;
    public MedidaService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<List<MedidaDTO>> Lista()
    {
        return await _httpClient.GetFromJsonAsync<List<MedidaDTO>>("api/medidas");
    }
}