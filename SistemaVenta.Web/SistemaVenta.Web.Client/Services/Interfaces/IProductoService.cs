using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoDTO>> Listar(string buscar = "");

        // --- MÉTODO AÑADIDO ---
        // Necesario para obtener un solo producto, por ejemplo, en la pantalla de ventas.
        Task<ProductoDTO> ObtenerPorCodigo(string codigo);

        Task<HttpResponseMessage> Crear(ProductoDTO producto);
        Task<HttpResponseMessage> Editar(ProductoDTO producto);
    }
}