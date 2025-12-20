namespace FIAPCloudGames.Domain.Models;
//========================================================
public class Contato
{
    #region Propriedades
    //--------------------------------------------------------
    public int IdContato { get; set; }
    //--------------------------------------------------------
    public Guid UsuarioId { get; set; }
    //--------------------------------------------------------
    public string Celular { get; set; }
    //--------------------------------------------------------
    public string Email { get; set; }
    //--------------------------------------------------------

    #endregion

    #region Listas e Objetos (Relacionamentos)
    //--------------------------------------------------------
    public Usuario Usuario { get; set; }
    //--------------------------------------------------------
    #endregion
}
//========================================================
