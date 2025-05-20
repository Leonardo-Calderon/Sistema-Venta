using SVRepository.Entities;

namespace SVServices.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> Lista(string buscar = "");
        Task<String> Crear(Categoria objeto);
        Task<String> Editar(Categoria objeto);
    }
}
