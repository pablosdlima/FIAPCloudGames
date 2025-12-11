using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Data.Data;
//=================================================================
public class Contexto : DbContext
{
    #region Construtor
    //------------------------------------------------------------------
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {

    }
    //------------------------------------------------------------------
    #endregion

}
//=================================================================
