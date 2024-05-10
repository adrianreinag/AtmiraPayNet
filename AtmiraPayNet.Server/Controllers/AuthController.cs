using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AtmiraPayNet.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService userService) : ControllerBase
    {
        private readonly IAuthService _userService = userService;

        [HttpPost]
        [Route("register")]
        async public Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            var response = await _userService.Register(request);

            return StatusCode(response.StatusCode, new { response.Message, token = response.Value });
        }

        [HttpPost]
        [Route("login")]
        async public Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var response = await _userService.Login(request);

            return StatusCode(response.StatusCode, new { response.Message, token=response.Value });
        }
    }
}
