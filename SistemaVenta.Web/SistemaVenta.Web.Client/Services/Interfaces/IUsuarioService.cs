using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista(string buscar);
        Task<HttpResponseMessage> Crear(UsuarioCrearDTO usuario);
        Task<HttpResponseMessage> Editar(UsuarioDTO usuario);
        Task<HttpResponseMessage> Eliminar(int id);
    }
}