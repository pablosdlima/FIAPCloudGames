using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;

public class ContatoService : GenericServices<Contato>, IContatoService
{
    private readonly IContatoRepository _contatoRepository;

    #region Construtor
    public ContatoService(
        IGenericEntityRepository<Contato> repository,
        IContatoRepository contatoRepository) : base(repository)
    {
        _contatoRepository = contatoRepository;
    }
    #endregion

    public List<Contato> ListarPorUsuario(Guid usuarioId)
    {
        return _contatoRepository.ListarPorUsuario(usuarioId);
    }
    public async Task<Contato> Cadastrar(Contato contato)
    {
        return await _repository.Insert(contato);
    }

    public async Task<(Contato? Contato, bool Success)> Atualizar(Contato contato)
    {
        var contatoExistente = _contatoRepository.BuscarPorIdEUsuario(contato.Id, contato.UsuarioId);

        if (contatoExistente == null)
        {
            return (null, false);
        }

        contatoExistente.Celular = contato.Celular;
        contatoExistente.Email = contato.Email;

        var resultado = _repository.Update(contatoExistente);

        return await Task.FromResult((resultado.entity, resultado.success));
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        var contato = _contatoRepository.BuscarPorIdEUsuario(id, usuarioId);

        if (contato == null)
        {
            return false;
        }

        return await _repository.DeleteById(id);
    }
}