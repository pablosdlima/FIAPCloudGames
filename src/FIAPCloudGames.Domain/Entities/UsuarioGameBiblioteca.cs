namespace FIAPCloudGames.Domain.Models;
//============================================================
public class UsuarioGameBiblioteca
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid IdUsuarioGame { get; set; }
    //--------------------------------------------------------
    public Guid UsuarioId { get; set; }
    //--------------------------------------------------------
    public Guid GameId { get; set; }
    //--------------------------------------------------------
    public string TipoAquisicao { get; set; }
    //--------------------------------------------------------
    public decimal PrecoAquisicao { get; set; }
    //--------------------------------------------------------
    public DateTime DataAquisicao { get; set; }
    //--------------------------------------------------------
    #endregion

    #region Listas e Objetos (Relacionamentos)
    //--------------------------------------------------------
    public Usuario Usuario { get; set; }
    //--------------------------------------------------------
    public Game Game { get; set; }
    //--------------------------------------------------------
    #endregion
}
//============================================================
