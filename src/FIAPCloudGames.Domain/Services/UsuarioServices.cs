using FIAPCloudGames.Domain.Dtos.Request;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;

public class UsuarioServices : GenericServices<Usuario>, IUsuarioService
{
    #region Construtor

    public UsuarioServices(IGenericEntity<Usuario> repository) : base(repository)
    {
    }

    #endregion

    public Usuario CadastrarUsuario(CadastrarUsuarioRequest request)
    {
        var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(request.Senha);
        var usuario = Usuario.Criar(request.Nome, senhaCriptografada);
        usuario.Contatos = [new Contato(request.Celular, request.Email)];
        usuario.UsuarioRoles = [new UsuarioRole((int)request.TipoUsuario)];
        usuario.Perfil = new UsuarioPerfil(request.NomeCompleto, request.DataNascimento, request.Pais, request.AvatarUrl);

        return Insert(usuario);
    }

    public async Task<Usuario> ValidarLogin(string usuario, string senha)
    {
        var usuarioResult = Get().Where(u => u.Nome == usuario).FirstOrDefault();

        if (usuarioResult == null)
        {
            throw new AutenticacaoException("Usuário não encontrado.");
        }

        if (!BCrypt.Net.BCrypt.Verify(senha, usuarioResult.Senha))
        {
            throw new AutenticacaoException("Senha incorreta.");
        }

        if (!usuarioResult.Ativo)
        {
            throw new AutenticacaoException("Usuário inativo.");
        }

        return usuarioResult;
    }
}