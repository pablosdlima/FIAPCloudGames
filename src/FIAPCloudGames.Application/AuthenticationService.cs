using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Exceptions;

namespace FIAPCloudGames.Application
{
    public class AuthenticationService : IAuthenticationService
    {
        public readonly IJwtGenerator _jwtGenerator;
        public AuthenticationService(IJwtGenerator jwtGenerator)
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
