// En: SistemaVenta.Web.Client/Services/Contracts/IAuthService.cs
using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<SessionDTO> Login(LoginDTO loginDto);
        Task Logout();
    }
}