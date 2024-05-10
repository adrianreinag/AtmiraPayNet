using AtmiraPayNet.Shared.DTO;
using AtmiraPayNet.Shared.Utils;

namespace AtmiraPayNet.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Response<string>> Register(RegisterDTO request);
        Task<Response<string>> Login(LoginDTO request);
    }
}
