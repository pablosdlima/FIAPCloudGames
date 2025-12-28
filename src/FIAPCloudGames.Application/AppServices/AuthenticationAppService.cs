using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Responses;
using FIAPCloudGames.Domain.Interfaces.Services;

namespace FIAPCloudGames.Application.AppServices
{
    public class AuthenticationAppService : IAuthenticationAppService
    {
        public readonly IJwtGenerator _jwtGenerator;
        public readonly IUsuarioService _usuarioService;

        public AuthenticationAppService(IJwtGenerator jwtGenerator, IUsuarioService usuarioService)
        {
            _jwtGenerator = jwtGenerator;
            _usuarioService = usuarioService;
        }

        public async Task<LoginResponse> Login(string usuario, string senha)
        {
            var usuarioResult = await _usuarioService.ValidarLogin(usuario, senha);

            var tokenJwt = _jwtGenerator.GenerateToken(usuarioResult);

            return new LoginResponse(tokenJwt);
        }
    }
}