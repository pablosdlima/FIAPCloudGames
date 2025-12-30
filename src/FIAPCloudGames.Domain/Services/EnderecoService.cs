using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;
//============================================================
public class EnderecoService : GenericServices<Endereco>, IEnderecoService
{
    #region Construtor
    //------------------------------------------------------------------------------
    public EnderecoService(IGenericEntityRepository<Endereco> repository) : base(repository)
    {
    }
    //------------------------------------------------------------------------------
    #endregion
}
//============================================================
