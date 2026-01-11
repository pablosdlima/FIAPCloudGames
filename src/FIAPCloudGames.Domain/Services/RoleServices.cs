using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;
using Microsoft.Extensions.Logging;

namespace FIAPCloudGames.Domain.Services;

public class RoleServices : GenericServices<Role>, IRoleServices
{
    private readonly ILogger<RoleServices> _logger;

    public RoleServices(
        IGenericEntityRepository<Role> repository,
        ILogger<RoleServices> logger) : base(repository)
    {
        _logger = logger;
    }

    public List<Role> ListarRoles()
    {
        return _repository.Get().ToList();
    }

    public async Task<(Role? Role, bool Success)> AtualizarRole(Role role)
    {
        var roleExistente = _repository.GetByIdInt(role.Id);
        if (roleExistente == null)
        {
            _logger.LogWarning("Role não encontrada para atualização | RoleId: {RoleId}", role.Id);
            return (null, false);
        }
        roleExistente.RoleName = role.RoleName;
        roleExistente.Description = role.Description;
        var resultado = _repository.Update(roleExistente);
        if (!resultado.success)
        {
            _logger.LogError("Falha ao atualizar role no repositório | RoleId: {RoleId}", role.Id);
        }
        return await Task.FromResult((resultado.entity, resultado.success));
    }
}
