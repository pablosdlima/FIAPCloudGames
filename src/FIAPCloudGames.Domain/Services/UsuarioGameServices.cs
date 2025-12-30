using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;
//===============================================================
public class UsuarioGameServices : GenericServices<UsuarioGameBiblioteca>, IUsuarioGameService
{
    #region Construtor
    //------------------------------------------------------------
    public UsuarioGameServices(IGenericEntityRepository<UsuarioGameBiblioteca> repository) : base(repository)
    {
    }
    //------------------------------------------------------------
    #endregion
}
//===============================================================
