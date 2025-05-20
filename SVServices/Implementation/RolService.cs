using SVRepository.Interfaces;
using SVRepository.Entities;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolService;

        public RolService(IRolRepository rolRepository)
        {
            _rolService = rolRepository;
        }
        public Task<List<Rol>> Lista()
        {
            return _rolService.Lista();
        }
    }
}
