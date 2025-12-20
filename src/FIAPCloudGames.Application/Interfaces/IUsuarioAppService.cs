using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Interfaces;
//==========================================================
public interface IUsuarioAppService
{
    //-------------------------------------------------------
    List<UsuarioDtos> Listar();
    //-------------------------------------------------------
    UsuarioDtos PorId(Guid id);
    //-------------------------------------------------------
    UsuarioDtos Inserir(UsuarioDtos dto);
    //-------------------------------------------------------
    UsuarioDtos Alterar(UsuarioDtos dto);
    //-------------------------------------------------------
    UsuarioDtos Inativar(Guid id);
    //-------------------------------------------------------
}
//==========================================================
