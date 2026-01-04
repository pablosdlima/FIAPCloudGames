using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioPerfil;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioPerfilAppService : IUsuarioPerfilAppService
{
    private readonly IUsuarioPerfilService _usuarioPerfilService;

    public UsuarioPerfilAppService(IUsuarioPerfilService usuarioPerfilService)
    {
        _usuarioPerfilService = usuarioPerfilService;
    }

    public async Task<BuscarUsuarioPerfilResponse?> BuscarPorUsuarioId(Guid usuarioId)
    {
        var perfil = _usuarioPerfilService.BuscarPorUsuarioId(usuarioId);

        if (perfil == null)
        {
            return null;
        }

        return new BuscarUsuarioPerfilResponse
        {
            Id = perfil.Id,
            UsuarioId = perfil.UsuarioId,
            NomeCompleto = perfil.NomeCompleto,
            DataNascimento = perfil.DataNascimento,
            Pais = perfil.Pais,
            AvatarUrl = perfil.AvatarUrl
        };
    }

    public async Task<BuscarUsuarioPerfilResponse> Cadastrar(CadastrarUsuarioPerfilRequest request)
    {
        var perfil = new UsuarioPerfil(
            request.NomeCompleto,
            request.DataNascimento,
            request.Pais,
            request.AvatarUrl)
        {
            UsuarioId = request.UsuarioId
        };

        var perfilCadastrado = await _usuarioPerfilService.Cadastrar(perfil);

        return new BuscarUsuarioPerfilResponse
        {
            Id = perfilCadastrado.Id,
            UsuarioId = perfilCadastrado.UsuarioId,
            NomeCompleto = perfilCadastrado.NomeCompleto,
            DataNascimento = perfilCadastrado.DataNascimento,
            Pais = perfilCadastrado.Pais,
            AvatarUrl = perfilCadastrado.AvatarUrl
        };
    }

    public async Task<(BuscarUsuarioPerfilResponse? Perfil, bool Success)> Atualizar(AtualizarUsuarioPerfilRequest request)
    {
        var perfil = new UsuarioPerfil(
            request.NomeCompleto,
            request.DataNascimento,
            request.Pais,
            request.AvatarUrl)
        {
            Id = request.Id,
            UsuarioId = request.UsuarioId
        };

        var (perfilAtualizado, sucesso) = await _usuarioPerfilService.Atualizar(perfil);

        if (!sucesso || perfilAtualizado == null)
        {
            return (null, false);
        }

        var response = new BuscarUsuarioPerfilResponse
        {
            Id = perfilAtualizado.Id,
            UsuarioId = perfilAtualizado.UsuarioId,
            NomeCompleto = perfilAtualizado.NomeCompleto,
            DataNascimento = perfilAtualizado.DataNascimento,
            Pais = perfilAtualizado.Pais,
            AvatarUrl = perfilAtualizado.AvatarUrl
        };

        return (response, true);
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        return await _usuarioPerfilService.Deletar(id, usuarioId);
    }
}
