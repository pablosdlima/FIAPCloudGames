using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioRole;

namespace FIAPCloudGames.Application.Interfaces;

public interface IUsuarioRoleAppService
{
    Task<IEnumerable<ListarRolesPorUsuarioResponse>> ListarRolesPorUsuario(ListarRolePorUsuarioRequest request);
    Task<bool> AlterarRoleUsuario(AlterarUsuarioRoleRequest request);
}