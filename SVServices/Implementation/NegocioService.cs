
using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    public class NegocioService : INegocioService
    {
        private readonly INegocioRepository _negocioRepository;
        public NegocioService(INegocioRepository negocioRepository)
        {
            _negocioRepository = negocioRepository;
        }

        public async Task<Negocio> Obtener()
        {
            return await _negocioRepository.Obtener();
        }
        public async Task Editar(Negocio objeto)
        {
            await _negocioRepository.Editar(objeto);
        }
    }
}
