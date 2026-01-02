using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;

public class UsuarioServices : GenericServices<Usuario>, IUsuarioService
{
    #region Construtor

    public UsuarioServices(IGenericEntityRepository<Usuario> repository) : base(repository)
    {
    }

    #endregion

    public async Task<Usuario> CadastrarUsuario(CadastrarUsuarioRequest request)
    {
        var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(request.Senha);
        var usuario = Usuario.Criar(request.Nome, senhaCriptografada);
        usuario.Contatos = [new Contato(request.Celular, request.Email)];
        usuario.UsuarioRoles = [new UsuarioRole((int)request.TipoUsuario)];
        usuario.Perfil = new UsuarioPerfil(request.NomeCompleto, request.DataNascimento, request.Pais, request.AvatarUrl);

        return await Insert(usuario);
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


    public async Task<bool> AlterarSenha(AlterarSenhaRequest request)
    {
        var usuarioResult = Get().Where(u => u.Id == request.Id).FirstOrDefault();

        if (usuarioResult == null)
        {
            throw new NotFoundException("Usuário não encontrado.");
        }

        var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(request.Senha);
        usuarioResult.AlterarSenha(senhaCriptografada);

        var (usuario, success) = await Update(usuarioResult);

        return success;
    }


    public async Task<AlterarStatusResponse> AlterarStatus(Guid Id)
    {
        var usuarioResult = Get().Where(u => u.Id == Id).FirstOrDefault();

        if (usuarioResult == null)
        {
            throw new NotFoundException("Usuário não encontrado.");
        }

        usuarioResult.AlterarStatus(usuarioResult.Ativo ? false : true);

        var (usuario, success) = await Update(usuarioResult);

        return new AlterarStatusResponse(usuario.Ativo ? "Ativo" : "Inativo");
    }


}