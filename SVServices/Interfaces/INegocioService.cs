using SVRepository.Entities;

namespace SVServices.Interfaces
{
    public interface INegocioService
    {
        Task<Negocio> Obtener();
        Task Editar(Negocio objeto);
    }
}
