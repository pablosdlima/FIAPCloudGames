using FIAPCloudGames.Domain.Interfaces.Services;

namespace FIAPCloudGames.Domain.Services
{
    public class AuthenticationServices : IAuthenticationService
    {
        private readonly IUsuarioService _usuarioService;

        public AuthenticationServices(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        public async Task<string> LoginAsync(string NomeUsuario, string Senha)
        {
            var usuario = _usuarioService.Get().Where(u => u.Nome == NomeUsuario).FirstOrDefault();

            var senhaDescriptografada = BCrypt.Net.BCrypt.Verify(Senha, usuario.Senha);

            //try
            //{
            //    var retorno = await _usuarioRepository.(NomeUsuario, Senha);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            return "";
        }
    }
}