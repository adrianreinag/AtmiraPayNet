using AtmiraPayNet.Shared.DTO;

namespace AtmiraPayNet.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDTO<string>> Register(RegisterDTO request);
        Task<ResponseDTO<string>> Login(LoginDTO request);
    }
}
