

using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    public interface IMenuRolRepository
    {
        Task<List<MenuRol>>Lista(int idRol);
    }
}
