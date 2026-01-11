using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FIAPCloudGames.Domain.Dtos.Responses.Contato;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioPerfil;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioPerfilAppService : IUsuarioPerfilAppService
{
    private readonly IUsuarioPerfilService _usuarioPerfilService;
    private readonly IUsuarioService _usuarioService;

    public UsuarioPerfilAppService(IUsuarioPerfilService usuarioPerfilService, IUsuarioService usuarioService)
    {
        _usuarioPerfilService = usuarioPerfilService;
        _usuarioService = usuarioService;
    }

    public async Task<BuscarUsuarioPerfilResponse?> BuscarPorUsuarioId(Guid usuarioId)
    {
        var usuarioExiste = _usuarioService.GetById(usuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado.");
        }

        var perfil = _usuarioPerfilService.BuscarPorUsuarioId(usuarioId);
        if (perfil == null)
        {
            throw new NotFoundException($"Perfil para o usuário com ID {usuarioId} não encontrado.");
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

    public async Task<List<BuscarUsuarioPerfilResponse>> ListarPaginacao(int take, int skip)
    {
        var usuariosPerfils = await _usuarioPerfilService.ListarPaginacao(take, skip);

        var usuariosResponse = usuariosPerfils.Select(c => new BuscarUsuarioPerfilResponse
        {
            Id = c.Id,
            UsuarioId = c.UsuarioId,
            NomeCompleto = c.NomeCompleto,
            DataNascimento = c.DataNascimento,
            Pais = c.Pais,
            AvatarUrl = c.AvatarUrl

        }).ToList();

        return await Task.FromResult(usuariosResponse);
    }

    public async Task<BuscarUsuarioPerfilResponse> Cadastrar(CadastrarUsuarioPerfilRequest request)
    {
        var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado para cadastrar o perfil.");
        }

        var perfil = new UsuarioPerfil(
            request.NomeCompleto,
            request.DataNascimento,
            request.Pais,
            request.AvatarUrl)
        {
            UsuarioId = request.UsuarioId
        };
        var perfilCadastrado = await _usuarioPerfilService.Cadastrar(perfil);

        if (perfilCadastrado == null)
        {
            throw new DomainException("Não foi possível cadastrar o perfil. Verifique os dados fornecidos ou se o usuário já possui um perfil.");
        }

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
        var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado para atualizar o perfil.");
        }

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
        var usuarioExiste = _usuarioService.GetById(usuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado para deletar o perfil.");
        }

        return await _usuarioPerfilService.Deletar(id, usuarioId);
    }
}