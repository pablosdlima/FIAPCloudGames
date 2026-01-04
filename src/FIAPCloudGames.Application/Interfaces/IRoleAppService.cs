using FIAPCloudGames.Domain.Dtos.Request.Role;

namespace FIAPCloudGames.Application.Interfaces;

public interface IRoleAppService
{
    List<CadastrarRoleRequest> Listar();
    CadastrarRoleRequest PorId(Guid id);
    CadastrarRoleRequest Alterar(CadastrarRoleRequest dto);
    Task<CadastrarRoleRequest> Cadastrar(CadastrarRoleRequest request);
    Task<List<RolesResponse>> ListarRoles();
    Task<(RolesResponse? Role, bool Success)> AtualizarRole(AtualizarRoleRequest request);
}