using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Dtos.Request;
using FIAPCloudGames.Domain.Dtos.Responses;

namespace FIAPCloudGames.Application.Interfaces;

public interface IUsuarioAppService
{

    List<UsuarioDtos> Listar();
    UsuarioDtos BuscarPorId(Guid id);
    Task<CadastrarUsuarioResponse> Cadastrar(CadastrarUsuarioRequest request);
    UsuarioDtos Alterar(UsuarioDtos dto);
    UsuarioDtos Inativar(Guid id);
}
