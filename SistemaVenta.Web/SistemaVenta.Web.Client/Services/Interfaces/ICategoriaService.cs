using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces;

public interface ICategoriaService
{
    Task<List<CategoriaDTO>> Lista(string buscar);
    Task<HttpResponseMessage> Crear(CategoriaDTO categoria);
    Task<HttpResponseMessage> Editar(CategoriaDTO categoria);
}