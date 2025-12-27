using AutoMapper;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Dtos.Request;
using FIAPCloudGames.Domain.Dtos.Responses;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioAppService : IUsuarioAppService
{
    #region Properties

    private readonly IUsuarioService _usuarioService;
    private readonly IMapper _mapper;

    #endregion

    #region Construtor
    //
    public UsuarioAppService(IUsuarioService usuarioService, IMapper mapper)
    {
        _usuarioService = usuarioService
            ?? throw new ArgumentNullException(nameof(usuarioService));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //
    #endregion


    public CadastrarUsuarioResponse Cadastrar(CadastrarUsuarioRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var usuario = Usuario.Criar(request.Nome, request.Senha);
        usuario.Contatos = [new Contato(request.Celular, request.Email)];
        usuario.UsuarioRoles = [new UsuarioRole((int)request.TipoUsuario)];

        var criado = _usuarioService.Insert(usuario);

        return new CadastrarUsuarioResponse() { IdUsuario = usuario.Id };
    }

    public UsuarioDtos Alterar(UsuarioDtos dto)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var entity = _mapper.Map<Usuario>(dto);
        var atualizado = _usuarioService.Update(entity);

        return _mapper.Map<UsuarioDtos>(atualizado);
    }

    public UsuarioDtos Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _UsuarioService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("Usuario não encontrado.");

        //return _mapper.Map<UsuarioDtos>(entity);
        throw new KeyNotFoundException("Usuario não encontrado.");
    }
    //
    public List<UsuarioDtos> Listar()
    {
        var lista = _usuarioService.Get();
        return _mapper.Map<List<UsuarioDtos>>(lista);
    }

    public UsuarioDtos BuscarPorId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id inválido.", nameof(id));
        }

        var entity = _usuarioService.GetById(id);

        if (entity is null)
        {
            throw new KeyNotFoundException("Usuario não encontrado.");
        }

        return _mapper.Map<UsuarioDtos>(entity);
    }
}