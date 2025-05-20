
using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    public class MedidaService : IMedidaService
    {
        private readonly IMedidaRepository _medidaRepository;
        public MedidaService(IMedidaRepository medidaRepository)
        {
            _medidaRepository = medidaRepository;
        }
        public async Task<List<Medida>> Lista()
        {
            return await _medidaRepository.Lista();
        }
    }
}
