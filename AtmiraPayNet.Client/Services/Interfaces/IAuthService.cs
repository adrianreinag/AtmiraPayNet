using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterDTO request);
        Task<bool> Login(LoginDTO request);
        Task Logout();
    }
}
