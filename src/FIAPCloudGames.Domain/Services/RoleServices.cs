using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;

public class RoleServices : GenericServices<Role>, IRoleServices
{
    public RoleServices(IGenericEntityRepository<Role> repository) : base(repository)
    {
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
            return (null, false);
        }

        roleExistente.RoleName = role.RoleName;
        roleExistente.Description = role.Description;

        var resultado = _repository.Update(roleExistente);

        return await Task.FromResult((resultado.entity, resultado.success));
    }
}