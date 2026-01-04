using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class EnderecoAppService : IEnderecoAppService
{
    private readonly IEnderecoService _enderecoService;

    public EnderecoAppService(IEnderecoService enderecoService)
    {
        _enderecoService = enderecoService;
    }

    public async Task<List<EnderecoResponse>> ListarPorUsuario(Guid usuarioId)
    {
        var enderecos = _enderecoService.ListarPorUsuario(usuarioId);

        var enderecosResponse = enderecos.Select(e => new EnderecoResponse
        {
            Id = e.Id,
            UsuarioId = e.UsuarioId,
            Rua = e.Rua,
            Numero = e.Numero,
            Complemento = e.Complemento,
            Bairro = e.Bairro,
            Cidade = e.Cidade,
            Estado = e.Estado,
            Cep = e.Cep
        }).ToList();

        return await Task.FromResult(enderecosResponse);
    }

    public async Task<EnderecoResponse> Cadastrar(CadastrarEnderecoRequest request)
    {
        var endereco = new Endereco
        {
            UsuarioId = request.UsuarioId,
            Rua = request.Rua,
            Numero = request.Numero,
            Complemento = request.Complemento,
            Bairro = request.Bairro,
            Cidade = request.Cidade,
            Estado = request.Estado,
            Cep = request.Cep
        };

        var enderecoCadastrado = await _enderecoService.Cadastrar(endereco);

        return new EnderecoResponse
        {
            Id = enderecoCadastrado.Id,
            UsuarioId = enderecoCadastrado.UsuarioId,
            Rua = enderecoCadastrado.Rua,
            Numero = enderecoCadastrado.Numero,
            Complemento = enderecoCadastrado.Complemento,
            Bairro = enderecoCadastrado.Bairro,
            Cidade = enderecoCadastrado.Cidade,
            Estado = enderecoCadastrado.Estado,
            Cep = enderecoCadastrado.Cep
        };
    }

    public async Task<(EnderecoResponse? Endereco, bool Success)> Atualizar(AtualizarEnderecoRequest request)
    {
        var endereco = new Endereco
        {
            Id = request.Id,
            UsuarioId = request.UsuarioId,
            Rua = request.Rua,
            Numero = request.Numero,
            Complemento = request.Complemento,
            Bairro = request.Bairro,
            Cidade = request.Cidade,
            Estado = request.Estado,
            Cep = request.Cep
        };

        var (enderecoAtualizado, sucesso) = await _enderecoService.Atualizar(endereco);

        if (!sucesso || enderecoAtualizado == null)
        {
            return (null, false);
        }

        var response = new EnderecoResponse
        {
            Id = enderecoAtualizado.Id,
            UsuarioId = enderecoAtualizado.UsuarioId,
            Rua = enderecoAtualizado.Rua,
            Numero = enderecoAtualizado.Numero,
            Complemento = enderecoAtualizado.Complemento,
            Bairro = enderecoAtualizado.Bairro,
            Cidade = enderecoAtualizado.Cidade,
            Estado = enderecoAtualizado.Estado,
            Cep = enderecoAtualizado.Cep
        };

        return (response, true);
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        return await _enderecoService.Deletar(id, usuarioId);
    }
}