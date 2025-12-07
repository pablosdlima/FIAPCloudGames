namespace FIAPCloudGames.Application.Interfaces
{
    public interface IAuthenticationService
    {
        string Login(string usuario, string role);
    }
}
