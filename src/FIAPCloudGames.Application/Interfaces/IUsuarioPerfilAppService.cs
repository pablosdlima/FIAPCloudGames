using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioPerfil;

namespace FIAPCloudGames.Application.Interfaces;

public interface IUsuarioPerfilAppService
{
    Task<BuscarUsuarioPerfilResponse?> BuscarPorUsuarioId(Guid usuarioId);
    Task<BuscarUsuarioPerfilResponse> Cadastrar(CadastrarUsuarioPerfilRequest request);
    Task<(BuscarUsuarioPerfilResponse? Perfil, bool Success)> Atualizar(AtualizarUsuarioPerfilRequest request);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}