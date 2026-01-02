using AutoMapper;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;
using FIAPCloudGames.Domain.Interfaces.Services;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioAppService : IUsuarioAppService
{
    #region Properties

    private readonly IUsuarioService _usuarioService;
    private readonly IMapper _mapper;

    #endregion

    #region Construtor

    public UsuarioAppService(IUsuarioService usuarioService, IMapper mapper)
    {
        _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #endregion


    public async Task<CadastrarUsuarioResponse> Cadastrar(CadastrarUsuarioRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var cadastroUsuarioResult = await _usuarioService.CadastrarUsuario(request);

        return new CadastrarUsuarioResponse() { IdUsuario = cadastroUsuarioResult.Id };
    }

    public List<UsuarioDtos> Listar()
    {
        var lista = _usuarioService.Get();
        return _mapper.Map<List<UsuarioDtos>>(lista);
    }

    public BuscarPorIdResponse BuscarPorId(Guid id)
    {
        var usuario = _usuarioService.GetById(id);

        if (usuario is null)
        {
            throw new KeyNotFoundException("Usuario não encontrado.");
        }

        var result = _mapper.Map<BuscarPorIdResponse>(usuario);

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