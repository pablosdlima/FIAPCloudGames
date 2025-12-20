using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Interfaces;
//====================================================
public interface IUsuarioPerfilAppService
{
    //-------------------------------------------------------
    List<UsuarioPerfilDto> Listar();
    //-------------------------------------------------------
    UsuarioPerfilDto PorId(Guid id);
    //-------------------------------------------------------
    UsuarioPerfilDto Inserir(UsuarioPerfilDto dto);
    //-------------------------------------------------------
    UsuarioPerfilDto Alterar(UsuarioPerfilDto dto);
    //-------------------------------------------------------
    UsuarioPerfilDto Inativar(Guid id);
    //-------------------------------------------------------

}
//====================================================
