using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Interfaces;
//===========================================================
public interface IContatoAppService
{
    //-------------------------------------------------------
    List<ContatosDtos> Listar();
    //-------------------------------------------------------
    ContatosDtos PorId(Guid id);
    //-------------------------------------------------------
    ContatosDtos Inserir(ContatosDtos dto);
    //-------------------------------------------------------
    ContatosDtos Alterar(ContatosDtos dto);
    //-------------------------------------------------------
    ContatosDtos Inativar(Guid id);
    //-------------------------------------------------------
}
//===========================================================
