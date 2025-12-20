namespace FIAPCloudGames.Application.Dtos;
//===============================================
public class GameDtos
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid IdGame { get; set; }
    //--------------------------------------------------------
    public string Nome { get; set; }
    //--------------------------------------------------------
    public string Descricao { get; set; }
    //--------------------------------------------------------
    public string Genero { get; set; }
    //--------------------------------------------------------
    public string Desenvolvedor { get; set; }
    //--------------------------------------------------------
    public DateTime DataRelease { get; set; }
    //--------------------------------------------------------
    public decimal Preco { get; set; }
    //--------------------------------------------------------
    public DateTime DataCriacao { get; set; }
    //--------------------------------------------------------
    #endregion
}
//===============================================
