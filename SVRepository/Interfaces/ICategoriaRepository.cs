using SVRepository.Entities;
namespace SVRepository.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<Categoria>> Lista(string buscar="");
        Task<String> Crear(Categoria objeto);
        Task<String> Editar(Categoria objeto);
    }
}
