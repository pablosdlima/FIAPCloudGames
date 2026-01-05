using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioAppService : IUsuarioAppService
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioAppService(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
    }


    public async Task<CadastrarUsuarioResponse> Cadastrar(CadastrarUsuarioRequest request)
    {
        var cadastroUsuarioResult = await _usuarioService.CadastrarUsuario(request);

        if (cadastroUsuarioResult == null)
        {
            throw new DomainException("Não foi possível cadastrar o usuário. Verifique os dados fornecidos.");
        }

        return new CadastrarUsuarioResponse() { IdUsuario = cadastroUsuarioResult.Id };
    }

    public BuscarPorIdResponse BuscarPorId(Guid id)
    {
        var usuario = _usuarioService.GetById(id);
        if (usuario is null)
        {
            throw new NotFoundException("Usuário não encontrado.");
        }

        var result = new BuscarPorIdResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Ativo = usuario.Ativo,
            DataCriacao = usuario.DataCriacao,
            DataAtualizacao = usuario.DataAtualizacao,
            Perfil = usuario.Perfil != null ? new UsuarioPerfilResponse
            {
                Id = usuario.Perfil.Id,
                NomeCompleto = usuario.Perfil.NomeCompleto,
                DataNascimento = usuario.Perfil.DataNascimento,
                Pais = usuario.Perfil.Pais,
                AvatarUrl = usuario.Perfil.AvatarUrl,
            } : null,
            Roles = usuario.UsuarioRoles?.Select(ur => new UsuarioRoleResponse
            {
                Id = ur.Id,
                RoleId = ur.RoleId,
                RoleName = ur.Role?.RoleName,
                Description = ur.Role?.Description,
            }).ToList() ?? []
        };

        return result;
    }

    public async Task<bool> AlterarSenha(AlterarSenhaRequest request)
    {
        return await _usuarioService.AlterarSenha(request);
    }

    public async Task<AlterarStatusResponse> AlterarStatus(Guid id)
    {
        return await _usuarioService.AlterarStatus(id);
    }
}