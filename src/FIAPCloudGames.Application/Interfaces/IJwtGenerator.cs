using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(Usuario usuario);
    }
}
