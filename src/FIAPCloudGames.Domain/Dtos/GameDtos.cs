namespace FIAPCloudGames.Domain.Dtos;
//=============================================
public class GameDtos
{
    #region Propriedades
    public Guid IdGame { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Genero { get; set; }
    public string Desenvolvedor { get; set; }
    public decimal Preco { get; set; }
    public DateTimeOffset? DataCriacao { get; set; }
    public DateTimeOffset? DataRelease { get; set; }
    #endregion

    #region Relacionamentos
    public UsuarioDtos? Usuario { get; set; }
    #endregion
}
//=============================================
