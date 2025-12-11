using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;
//=============================================================================
public class ContatoService : GenericServices<Contato>, IContatoService
{
    #region Construtor
    //-------------------------------------------------------------------------
    public ContatoService(IGenericEntity<Contato> repository) : base(repository)
    {
    }
    //-------------------------------------------------------------------------
    #endregion
}
//=============================================================================
