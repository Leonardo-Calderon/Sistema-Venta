using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    public interface IRolService
    {
        Task<List<RolDTO>> Lista();
    }
}