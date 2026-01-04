using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;

namespace FIAPCloudGames.Application.Interfaces;

public interface IUsuarioAppService
{
    BuscarPorIdResponse BuscarPorId(Guid id);
    Task<CadastrarUsuarioResponse> Cadastrar(CadastrarUsuarioRequest request);
    Task<bool> AlterarSenha(AlterarSenhaRequest request);
    Task<AlterarStatusResponse> AlterarStatus(Guid id);
}
