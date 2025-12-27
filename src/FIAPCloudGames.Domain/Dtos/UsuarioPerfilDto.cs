namespace FIAPCloudGames.Application.Dtos;
//====================================================
public class UsuarioPerfilDto
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid IdPerfil { get; set; }
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
}
//====================================================
