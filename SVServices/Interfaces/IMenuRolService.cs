

using SVRepository.Entities;

namespace SVServices.Interfaces
{
    public interface IMenuRolService
    {
        Task<List<MenuRol>> Lista(int idRol);
    }
}
