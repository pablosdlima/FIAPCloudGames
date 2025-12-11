using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;
//===========================================================
public class UsuarioRoleServices : GenericServices<UsuarioRole>, IUsuarioRoleServices
{
    #region Construtor
    //----------------------------------------------------------------------
    public UsuarioRoleServices(IGenericEntity<UsuarioRole> repository) : base(repository)
    {
    }
    //----------------------------------------------------------------------
    #endregion
}
//===========================================================
