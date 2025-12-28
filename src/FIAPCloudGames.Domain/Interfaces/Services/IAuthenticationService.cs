namespace FIAPCloudGames.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(string NomeUsuario, string Senha);
    }
}
