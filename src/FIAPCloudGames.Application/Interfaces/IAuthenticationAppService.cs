using FIAPCloudGames.Domain.Dtos.Responses.Authentication;

namespace FIAPCloudGames.Application.Interfaces
{
    public interface IAuthenticationAppService
    {
        Task<LoginResponse> Login(string usuario, string senha);
    }
}
