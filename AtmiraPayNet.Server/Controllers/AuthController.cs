using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AtmiraPayNet.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost]
        [Route("register")]
        async public Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            var response = await _authService.Register(request);

            return StatusCode(response.StatusCode, new { token = response.Value, response.Message });
        }

        [HttpPost]
        [Route("login")]
        async public Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var response = await _authService.Login(request);

            return StatusCode(response.StatusCode, new { token = response.Value, response.Message });
        }
    }
}
