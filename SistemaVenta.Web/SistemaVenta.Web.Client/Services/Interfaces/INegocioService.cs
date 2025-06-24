using Shared.DTOs;
using System.Threading.Tasks;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    public interface INegocioService
    {
        Task<NegocioDTO> Obtener();
        Task<NegocioDTO> GuardarCambios(MultipartFormDataContent model);
    }
}