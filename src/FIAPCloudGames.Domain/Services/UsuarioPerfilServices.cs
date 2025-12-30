using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;
//======================================================================
public class UsuarioPerfilServices : GenericServices<UsuarioPerfil>, IUsuarioPerfilService
{
    #region Construtor
    //------------------------------------------------------------------
    public UsuarioPerfilServices(IGenericEntityRepository<UsuarioPerfil> repository) : base(repository)
    {
    }
    //------------------------------------------------------------------
    #endregion
}
//======================================================================
