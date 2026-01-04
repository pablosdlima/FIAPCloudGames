using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;

namespace FIAPCloudGames.Application.Interfaces;

public interface IUsuarioPerfilAppService
{
    Task<UsuarioPerfilResponse?> BuscarPorUsuarioId(Guid usuarioId);
    Task<UsuarioPerfilResponse> Cadastrar(CadastrarUsuarioPerfilRequest request);
    Task<(UsuarioPerfilResponse? Perfil, bool Success)> Atualizar(AtualizarUsuarioPerfilRequest request);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}