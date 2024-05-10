using AtmiraPayNet.Server.Models;

namespace AtmiraPayNet.Server.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        Guid GetCurrentUserId();
    }
}
