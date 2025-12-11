using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;
//===============================================================
public class UsuarioServices : GenericServices<Usuario>, IUsuarioService
{
    #region Construtor
    //-----------------------------------------------------------
    public UsuarioServices(IGenericEntity<Usuario> repository) : base(repository)
    {
    }
    //-----------------------------------------------------------
    #endregion
}
//===============================================================
