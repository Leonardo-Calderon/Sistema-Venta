using SVRepository.Entities;

namespace SVServices.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> Lista(string buscar = "");
        Task<string> Crear(Producto objeto);
        Task<string> Editar(Producto objeto);
        Task<Producto> Obtener(string codigo);
    }
}
