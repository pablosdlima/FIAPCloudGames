using FIAPCloudGames.Domain.Dtos.Request.Role;
using FIAPCloudGames.Domain.Dtos.Responses.Role;

namespace FIAPCloudGames.Application.Interfaces;

public interface IRoleAppService
{
    Task<RolesResponse> Cadastrar(CadastrarRoleRequest request);
    Task<List<RolesResponse>> ListarRoles();
    Task<(RolesResponse? Role, bool Success)> AtualizarRole(AtualizarRoleRequest request);
}