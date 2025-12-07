using FIAPCloudGames.Application.Interfaces;

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
            return _jwtGenerator.GenerateToken("teste", "usuario");
        }
    }
}
