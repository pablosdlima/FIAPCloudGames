using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Interfaces;
//===============================================================
public interface IUsuarioGameAppService
{
    //-------------------------------------------------------
    List<UsuarioGameBibliotecaDto> Listar();
    //-------------------------------------------------------
    UsuarioGameBibliotecaDto PorId(Guid id);
    //-------------------------------------------------------
    UsuarioGameBibliotecaDto Inserir(UsuarioGameBibliotecaDto dto);
    //-------------------------------------------------------
    UsuarioGameBibliotecaDto Alterar(UsuarioGameBibliotecaDto dto);
    //-------------------------------------------------------
    UsuarioGameBibliotecaDto Inativar(Guid id);
    //-------------------------------------------------------
}
//===============================================================
