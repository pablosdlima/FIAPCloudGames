using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Interfaces;
//===================================================
public interface IUsuarioRoleAppService
{
    //-------------------------------------------------------
    List<UsuarioRoleDto> Listar();
    //-------------------------------------------------------
    UsuarioRoleDto PorId(Guid id);
    //-------------------------------------------------------
    UsuarioRoleDto Inserir(UsuarioRoleDto dto);
    //-------------------------------------------------------
    UsuarioRoleDto Alterar(UsuarioRoleDto dto);
    //-------------------------------------------------------
    UsuarioRoleDto Inativar(Guid id);
    //-------------------------------------------------------
}
//===================================================
