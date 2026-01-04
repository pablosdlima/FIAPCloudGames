using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Services;

public interface IUsuarioPerfilService : IGenericServices<UsuarioPerfil>
{
    UsuarioPerfil? BuscarPorUsuarioId(Guid usuarioId);
    Task<UsuarioPerfil> Cadastrar(UsuarioPerfil perfil);
    Task<(UsuarioPerfil? Perfil, bool Success)> Atualizar(UsuarioPerfil perfil);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}