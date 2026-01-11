using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Dtos.Responses.Contato;
using FIAPCloudGames.Domain.Dtos.Responses.Endereco;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;

namespace FIAPCloudGames.Application.AppServices;

public class EnderecoAppService : IEnderecoAppService
{
    private readonly IEnderecoService _enderecoService;
    private readonly IUsuarioService _usuarioService;

    public EnderecoAppService(IEnderecoService enderecoService, IUsuarioService usuarioService)
    {
        _enderecoService = enderecoService;
        _usuarioService = usuarioService;
    }

    public async Task<List<EnderecoResponse>> ListarPorUsuario(Guid usuarioId)
    {
        var usuarioExiste = _usuarioService.GetById(usuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado.");
        }

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

    public async Task<List<EnderecoResponse>> ListarPaginacao(int take, int skip)
    {
        var enderecos = await _enderecoService.ListarPaginacao(take, skip);

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
        var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado para cadastrar o endereço.");
        }

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

        if (enderecoCadastrado == null)
        {
            throw new DomainException("Não foi possível cadastrar o endereço. Verifique os dados fornecidos.");
        }

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
        var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado para atualizar o endereço.");
        }

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
        var usuarioExiste = _usuarioService.GetById(usuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado para deletar o endereço.");
        }

        return await _enderecoService.Deletar(id, usuarioId);
    }
}
