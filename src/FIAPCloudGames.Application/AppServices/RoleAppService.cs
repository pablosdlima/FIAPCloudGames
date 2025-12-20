using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;
//=======================================================
public class RoleAppService : IRoleAppService
{
    #region Properties
    //-------------------------------------------------
    private readonly IRoleServices _roleService;
    private readonly IMapper _mapper;
    //-------------------------------------------------
    #endregion

    #region Construtor
    //-------------------------------------------------
    public RoleAppService(IRoleServices roleService, IMapper mapper)
    {
        _roleService = roleService
            ?? throw new ArgumentNullException(nameof(roleService));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //-------------------------------------------------
    #endregion

    #region Interfaces
    //-------------------------------------------------
    public RoleDtos Inserir(RoleDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Role>(dto);
        var criado = _roleService.Insert(entity);

        return _mapper.Map<RoleDtos>(criado);
    }
    //-------------------------------------------------
    public RoleDtos Alterar(RoleDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Role>(dto);
        var atualizado = _roleService.Update(entity);

        return _mapper.Map<RoleDtos>(atualizado);
    }
    //-------------------------------------------------
    public RoleDtos Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _roleService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("role não encontrado.");

        //return _mapper.Map<rolesDtos>(entity);
        throw new KeyNotFoundException("Role não encontrado.");
    }
    //-------------------------------------------------
    public List<RoleDtos> Listar()
    {
        var lista = _roleService.Get();
        return _mapper.Map<List<RoleDtos>>(lista);
    }
    //-------------------------------------------------
    public RoleDtos PorId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.", nameof(id));

        var entity = _roleService.GetById(id);

        if (entity is null)
            throw new KeyNotFoundException("role não encontrado.");

        return _mapper.Map<RoleDtos>(entity);
    }
    //-------------------------------------------------
    #endregion
}
//=======================================================
