using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Responses.Contato;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class ContatoAppService : IContatoAppService
{
    private readonly IContatoService _contatoService;
    private readonly IUsuarioService _usuarioService;

    public ContatoAppService(IContatoService contatoService, IUsuarioService usuarioService)
    {
        _contatoService = contatoService;
        _usuarioService = usuarioService;
    }

    public async Task<List<ContatoResponse>> ListarPorUsuario(Guid usuarioId)
    {
        var usuarioExiste = _usuarioService.GetById(usuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado.");
        }

        var contatos = _contatoService.ListarPorUsuario(usuarioId);
        var contatosResponse = contatos.Select(c => new ContatoResponse
        {
            Id = c.Id,
            UsuarioId = c.UsuarioId,
            Celular = c.Celular,
            Email = c.Email
        }).ToList();
        return await Task.FromResult(contatosResponse);
    }

    public async Task<ContatoResponse> Cadastrar(CadastrarContatoRequest request)
    {
        var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado para cadastrar o contato.");
        }

        var contato = new Contato(request.Celular, request.Email)
        {
            UsuarioId = request.UsuarioId
        };

        var contatoCadastrado = await _contatoService.Cadastrar(contato);

        if (contatoCadastrado == null)
        {
            throw new DomainException("Não foi possível cadastrar o contato. Verifique os dados fornecidos.");
        }

        return new ContatoResponse
        {
            Id = contatoCadastrado.Id,
            UsuarioId = contatoCadastrado.UsuarioId,
            Celular = contatoCadastrado.Celular,
            Email = contatoCadastrado.Email
        };
    }

    public async Task<(ContatoResponse? Contato, bool Success)> Atualizar(AtualizarContatoRequest request)
    {
        var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado para atualizar o contato.");
        }

        var contato = new Contato(request.Celular, request.Email)
        {
            Id = request.Id,
            UsuarioId = request.UsuarioId
        };
        var (contatoAtualizado, sucesso) = await _contatoService.Atualizar(contato);
        if (!sucesso || contatoAtualizado == null)
        {
            return (null, false);
        }
        var response = new ContatoResponse
        {
            Id = contatoAtualizado.Id,
            UsuarioId = contatoAtualizado.UsuarioId,
            Celular = contatoAtualizado.Celular,
            Email = contatoAtualizado.Email
        };
        return (response, true);
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        var usuarioExiste = _usuarioService.GetById(usuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado para deletar o contato.");
        }

        return await _contatoService.Deletar(id, usuarioId);
    }
}