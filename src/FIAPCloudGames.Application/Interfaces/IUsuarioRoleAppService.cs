using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioRole;

namespace FIAPCloudGames.Application.Interfaces;

public interface IUsuarioRoleAppService
{
    List<UsuarioRoleRequest> Listar();
    UsuarioRoleRequest PorId(Guid id);
    UsuarioRoleRequest Inserir(UsuarioRoleRequest dto);
    UsuarioRoleRequest Alterar(UsuarioRoleRequest dto);
    Task<IEnumerable<ListarRolesPorUsuarioResponse>> ListarRolesPorUsuario(ListarRolePorUsuarioRequest request);
}