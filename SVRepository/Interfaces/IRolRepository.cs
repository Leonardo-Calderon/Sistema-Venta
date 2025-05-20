
using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    public interface IRolRepository
    {
        Task<List<Rol>> Lista();
    }
}
