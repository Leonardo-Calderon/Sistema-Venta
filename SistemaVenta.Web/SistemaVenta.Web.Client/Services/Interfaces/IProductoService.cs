using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoDTO>> Listar(string buscar = "");
        Task<ProductoDTO> ObtenerPorCodigo(string codigo);

        Task<HttpResponseMessage> Crear(ProductoDTO producto);
        Task<HttpResponseMessage> Editar(ProductoDTO producto);
    }
}