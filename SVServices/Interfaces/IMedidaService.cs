using SVRepository.Entities;

namespace SVServices.Interfaces
{
    public interface IMedidaService
    {
        Task<List<Medida>> Lista();
    }
}
