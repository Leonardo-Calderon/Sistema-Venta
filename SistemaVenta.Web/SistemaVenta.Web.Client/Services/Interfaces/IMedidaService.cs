using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces;

public interface IMedidaService
{
    Task<List<MedidaDTO>> Lista();
}