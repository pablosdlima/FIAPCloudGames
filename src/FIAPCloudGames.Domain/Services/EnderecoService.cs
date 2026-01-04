using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;

public class EnderecoService : GenericServices<Endereco>, IEnderecoService
{
    private readonly IEnderecoRepository _enderecoRepository;

    public EnderecoService(IGenericEntityRepository<Endereco> repository, IEnderecoRepository enderecoRepository) : base(repository)
    {
        _enderecoRepository = enderecoRepository;
    }

    public List<Endereco> ListarPorUsuario(Guid usuarioId)
    {
        return _enderecoRepository.ListarPorUsuario(usuarioId);
    }

    public async Task<Endereco> Cadastrar(Endereco endereco)
    {
        endereco.Id = Guid.NewGuid();
        return await _repository.Insert(endereco);
    }

    public async Task<(Endereco? Endereco, bool Success)> Atualizar(Endereco endereco)
    {
        var enderecoExistente = _enderecoRepository.BuscarPorIdEUsuario(endereco.Id, endereco.UsuarioId);

        if (enderecoExistente == null)
        {
            return (null, false);
        }

        enderecoExistente.Rua = endereco.Rua;
        enderecoExistente.Numero = endereco.Numero;
        enderecoExistente.Complemento = endereco.Complemento;
        enderecoExistente.Bairro = endereco.Bairro;
        enderecoExistente.Cidade = endereco.Cidade;
        enderecoExistente.Estado = endereco.Estado;
        enderecoExistente.Cep = endereco.Cep;

        var resultado = _repository.Update(enderecoExistente);

        return await Task.FromResult((resultado.entity, resultado.success));
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        var endereco = _enderecoRepository.BuscarPorIdEUsuario(id, usuarioId);

        if (endereco == null)
        {
            return false;
        }

        return await _repository.DeleteById(id);
    }
}