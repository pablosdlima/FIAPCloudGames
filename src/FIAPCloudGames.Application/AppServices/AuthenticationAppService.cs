using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Exceptions;

namespace FIAPCloudGames.Application.AppServices
{
    public class AuthenticationAppService : IAuthenticationService
    {
        public readonly IJwtGenerator _jwtGenerator;
        public AuthenticationAppService(IJwtGenerator jwtGenerator)
        {
            _jwtGenerator = jwtGenerator;
        }

        public string Login(string usuario, string role)
        {
            throw new UnauthorizedException("Usuário inválido");

            return _jwtGenerator.GenerateToken("teste", "usuario");
        }
    }
}
