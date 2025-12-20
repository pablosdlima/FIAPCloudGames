using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Interfaces;
//====================================================
public interface IEnderecoAppService
{ 
    //-------------------------------------------------------
    List<EnderecoDtos> Listar();
    //-------------------------------------------------------
    EnderecoDtos PorId(Guid id);
    //-------------------------------------------------------
    EnderecoDtos Inserir(EnderecoDtos dto);
    //-------------------------------------------------------
    EnderecoDtos Alterar(EnderecoDtos dto);
    //-------------------------------------------------------
    EnderecoDtos Inativar(Guid id);
    //-------------------------------------------------------
}
//====================================================
