using AtmiraPayNet.Server.Models;
using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using AtmiraPayNet.Shared.Utils;
using Microsoft.AspNetCore.Identity;

namespace AtmiraPayNet.Server.Services
{
    public class AuthService(ITokenService tokenService, UserManager<User> userManager) : IAuthService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<Response<string>> Register(RegisterDTO request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName!);

                if (user != null)
                {
                    return new Response<string>
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        Message = "El nombre de usuario ya está en uso."
                    };
                }

                var newUser = new User
                {
                    UserName = request.UserName!,
                    Fullname = request.Fullname!,
                    Payments = [],
                    DateOfBirth = (DateOnly)request.DateOfBirth!
                };

                await _userManager.CreateAsync(newUser, request.Password!);

                string token = _tokenService.CreateToken(newUser);

                return new Response<string>
                {
                    StatusCode = StatusCodes.Status201Created,
                    Value = token,
                    Message = "Usuario creado correctamente."
                };
            }
            catch (Exception e)
            {
                return new Response<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }

        public async Task<Response<string>> Login(LoginDTO request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName!);

                if (user == null || await _userManager.CheckPasswordAsync(user, request.Password!) == false)
                {
                    return new Response<string>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Correo electrónico o contraseña incorrectos."
                    };
                }
                else
                {
                    string token = _tokenService.CreateToken(user);

                    return new Response<string>
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Value = token,
                        Message = "Se ha iniciado sesión correctamente."
                    };
                }
            }
            catch (Exception e)
            {
                return new Response<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }
    }
}
