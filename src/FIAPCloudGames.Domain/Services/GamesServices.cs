using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;
//==================================================================================
public class GamesServices : GenericServices<Game>, IGameService
{
    #region Construtor
    //------------------------------------------------------------------------------
    public GamesServices(IGenericEntity<Game> repository) : base(repository)
    {
    }
    //------------------------------------------------------------------------------
    #endregion
}
//==================================================================================
