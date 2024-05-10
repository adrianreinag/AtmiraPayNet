using AtmiraPayNet.Server.Models;
using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using Microsoft.AspNetCore.Identity;

namespace AtmiraPayNet.Server.Services
{
    public class AuthService(ITokenService tokenService, UserManager<User> userManager) : IAuthService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ResponseDTO<string>> Register(RegisterDTO request)
        {
            try
            {
                User? user = await _userManager.FindByNameAsync(request.UserName!);

                if (user != null)
                {
                    return new ResponseDTO<string>
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        Message = "El nombre de usuario ya está en uso."
                    };
                }

                User newUser = new()
                {
                    UserName = request.UserName!,
                    FullName = request.Fullname!,
                    Payments = [],
                    DateOfBirth = (DateOnly)request.DateOfBirth!
                };

                await _userManager.CreateAsync(newUser, request.Password!);

                string token = _tokenService.CreateToken(newUser);

                return new ResponseDTO<string>
                {
                    StatusCode = StatusCodes.Status201Created,
                    Value = token,
                    Message = "Usuario creado correctamente."
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }

        public async Task<ResponseDTO<string>> Login(LoginDTO request)
        {
            try
            {
                User? user = await _userManager.FindByNameAsync(request.UserName!);

                if (user == null || await _userManager.CheckPasswordAsync(user, request.Password!) == false)
                {
                    return new ResponseDTO<string>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Correo electrónico o contraseña incorrectos."
                    };
                }
                else
                {
                    string token = _tokenService.CreateToken(user);

                    return new ResponseDTO<string>
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Value = token,
                        Message = "Se ha iniciado sesión correctamente."
                    };
                }
            }
            catch (Exception e)
            {
                return new ResponseDTO<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }
    }
}
