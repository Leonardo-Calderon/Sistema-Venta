using SistemaVenta.Web.Client.Services.Interfaces;
using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    public class RemoteAuthService : IAuthService
    {
        public Task<SessionDTO> Login(LoginDTO loginDto)
        {
            // Lógica vacía o throw durante prerenderizado
            throw new NotImplementedException("No disponible durante prerrenderizado");
        }

        public Task Logout()
        {
            throw new NotImplementedException("No disponible durante prerrenderizado");
        }
    }
}
