using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> Lista(string buscar = "");
        Task<String> Crear(Producto objeto);
        Task<String> Editar(Producto objeto);
        Task<Producto> Obtener(string codigo);
    }
}
