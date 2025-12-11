using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;
//=======================================================================
public class RoleServices : GenericServices<Role>, IRoleServices
{
    #region Construtor
    //-------------------------------------------------------------------
    public RoleServices(IGenericEntity<Role> repository) : base(repository)
    {
    }
    //-------------------------------------------------------------------
    #endregion
}
//=======================================================================
