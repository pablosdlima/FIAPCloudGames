namespace FIAPCloudGames.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(string usuario, string role);
    }
}
