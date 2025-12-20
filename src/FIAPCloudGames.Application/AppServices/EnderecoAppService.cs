using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;

namespace FIAPCloudGames.Application.AppServices;
//===================================================
public class EnderecoAppService : IEnderecoAppService
{
    #region Properties
    //-------------------------------------------------
    private readonly IEnderecoService _enderecoService;
    private readonly IMapper _mapper;
    //-------------------------------------------------
    #endregion

    #region Construtor
    //-------------------------------------------------
    public EnderecoAppService(IEnderecoService enderecoService, IMapper mapper)
    {
        _enderecoService = enderecoService
            ?? throw new ArgumentNullException(nameof(enderecoService));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //-------------------------------------------------
    #endregion

    #region Interfaces
    //-------------------------------------------------
    public EnderecoDtos Inserir(EnderecoDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Endereco>(dto);
        var criado = _enderecoService.Insert(entity);

        return _mapper.Map<EnderecoDtos>(criado);
    }
    //-------------------------------------------------
    public EnderecoDtos Alterar(EnderecoDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Endereco>(dto);
        var atualizado = _enderecoService.Update(entity);

        return _mapper.Map<EnderecoDtos>(atualizado);
    }
    //-------------------------------------------------
    public EnderecoDtos Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _contatoService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("Contato não encontrado.");

        //return _mapper.Map<ContatosDtos>(entity);
        throw new KeyNotFoundException("Endereços não encontrado.");
    }
    //-------------------------------------------------
    public List<EnderecoDtos> Listar()
    {
        var lista = _enderecoService.Get();
        return _mapper.Map<List<EnderecoDtos>>(lista);
    }
    //-------------------------------------------------
    public EnderecoDtos PorId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.", nameof(id));

        var entity = _enderecoService.GetById(id);

        if (entity is null)
            throw new KeyNotFoundException("Endereços não encontrado.");

        return _mapper.Map<EnderecoDtos>(entity);
    }
    //-------------------------------------------------
    #endregion
}
//===================================================
