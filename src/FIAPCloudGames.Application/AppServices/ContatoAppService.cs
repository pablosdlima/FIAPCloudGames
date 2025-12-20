using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;
//=====================================================
public class ContatoAppService : IContatoAppService
{
    #region Properties
    //-------------------------------------------------
    private readonly IContatoService _contatoService;
    private readonly IMapper _mapper;
    //-------------------------------------------------
    #endregion

    #region Construtor
    //-------------------------------------------------
    public ContatoAppService(IContatoService contatoService, IMapper mapper)
    {
        _contatoService = contatoService
            ?? throw new ArgumentNullException(nameof(contatoService));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //-------------------------------------------------
    #endregion

    #region Interfaces
    //-------------------------------------------------
    public ContatosDtos Inserir(ContatosDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Contato>(dto);
        var criado = _contatoService.Insert(entity);

        return _mapper.Map<ContatosDtos>(criado);
    }
    //-------------------------------------------------
    public ContatosDtos Alterar(ContatosDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Contato>(dto);
        var atualizado = _contatoService.Update(entity);

        return _mapper.Map<ContatosDtos>(atualizado);
    }
    //-------------------------------------------------
    public ContatosDtos Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _contatoService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("Contato não encontrado.");

        //return _mapper.Map<ContatosDtos>(entity);
        throw new KeyNotFoundException("Contato não encontrado.");
    }
    //-------------------------------------------------
    public List<ContatosDtos> Listar()
    {
        var lista = _contatoService.Get();
        return _mapper.Map<List<ContatosDtos>>(lista);
    }
    //-------------------------------------------------
    public ContatosDtos PorId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.", nameof(id));

        var entity = _contatoService.GetById(id);

        if (entity is null)
            throw new KeyNotFoundException("Contato não encontrado.");

        return _mapper.Map<ContatosDtos>(entity);
    }
    //-------------------------------------------------
    #endregion
}
//=====================================================
