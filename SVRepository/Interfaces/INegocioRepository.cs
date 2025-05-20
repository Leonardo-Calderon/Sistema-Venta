
using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    public interface INegocioRepository
    {
        Task<Negocio> Obtener();
        Task Editar(Negocio objeto);
    }
}
