using FIAPCloudGames.Domain.Dtos;

namespace FIAPCloudGames.Application.Dtos;
//==============================================
public class UsuarioGameBibliotecaDto
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

    #region Relacionamentos
    public virtual UsuarioDtos? Usuario { get; set; }
    #endregion
}
//==============================================
