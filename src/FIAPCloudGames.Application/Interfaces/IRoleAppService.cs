using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Interfaces;
//========================================================
public interface IRoleAppService
{
    //-------------------------------------------------------
    List<RoleDtos> Listar();
    //-------------------------------------------------------
    RoleDtos PorId(Guid id);
    //-------------------------------------------------------
    RoleDtos Inserir(RoleDtos dto);
    //-------------------------------------------------------
    RoleDtos Alterar(RoleDtos dto);
    //-------------------------------------------------------
    RoleDtos Inativar(Guid id);
    //-------------------------------------------------------
}
//========================================================
