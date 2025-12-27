using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Dtos.Request;
using FIAPCloudGames.Domain.Dtos.Responses;

namespace FIAPCloudGames.Application.Interfaces;

public interface IUsuarioAppService
{

    List<UsuarioDtos> Listar();
    UsuarioDtos BuscarPorId(Guid id);
    CadastrarUsuarioResponse Cadastrar(CadastrarUsuarioRequest dto);
    UsuarioDtos Alterar(UsuarioDtos dto);
    UsuarioDtos Inativar(Guid id);
}
