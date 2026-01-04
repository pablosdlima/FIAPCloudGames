using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class ContatoAppService : IContatoAppService
{
    private readonly IContatoService _contatoService;

    public ContatoAppService(IContatoService contatoService)
    {
        _contatoService = contatoService;
    }

    public async Task<List<ContatoResponse>> ListarPorUsuario(Guid usuarioId)
    {
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
        var contato = new Contato(request.Celular, request.Email)
        {
            UsuarioId = request.UsuarioId
        };

        var contatoCadastrado = await _contatoService.Cadastrar(contato);

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
        return await _contatoService.Deletar(id, usuarioId);
    }
}