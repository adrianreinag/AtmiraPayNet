using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> Register(RegisterDTO request);
        Task<string?> Login(LoginDTO request);
    }
}
