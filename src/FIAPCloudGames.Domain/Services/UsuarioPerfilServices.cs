using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;


public class UsuarioPerfilServices : GenericServices<UsuarioPerfil>, IUsuarioPerfilService
{
    private readonly IUsuarioPerfilRepository _usuarioPerfilRepository;

    #region Construtor
    public UsuarioPerfilServices(IGenericEntityRepository<UsuarioPerfil> repository, IUsuarioPerfilRepository usuarioPerfilRepository) : base(repository)
    {
        _usuarioPerfilRepository = usuarioPerfilRepository;
    }
    #endregion

    public UsuarioPerfil? BuscarPorUsuarioId(Guid usuarioId)
    {
        return _usuarioPerfilRepository.BuscarPorUsuarioId(usuarioId);
    }

    public async Task<UsuarioPerfil> Cadastrar(UsuarioPerfil perfil)
    {
        return await _repository.Insert(perfil);
    }

    public async Task<(UsuarioPerfil? Perfil, bool Success)> Atualizar(UsuarioPerfil perfil)
    {
        var perfilExistente = _usuarioPerfilRepository.BuscarPorIdEUsuario(perfil.Id, perfil.UsuarioId);

        if (perfilExistente == null)
        {
            return (null, false);
        }

        perfilExistente.NomeCompleto = perfil.NomeCompleto;
        perfilExistente.DataNascimento = perfil.DataNascimento;
        perfilExistente.Pais = perfil.Pais;
        perfilExistente.AvatarUrl = perfil.AvatarUrl;

        var resultado = _repository.Update(perfilExistente);

        return await Task.FromResult((resultado.entity, resultado.success));
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        var perfil = _usuarioPerfilRepository.BuscarPorIdEUsuario(id, usuarioId);

        if (perfil == null)
        {
            return false;
        }

        return await _repository.DeleteById(id);
    }
}
