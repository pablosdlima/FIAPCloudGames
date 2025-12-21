namespace FIAPCloudGames.Domain.Models;
//=======================================================
public class Game
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid Id { get; set; }
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

    #region Listas e Objetos (Relacionamentos)
    //--------------------------------------------------------
    public ICollection<UsuarioGameBiblioteca> Biblioteca { get; set; }
    //--------------------------------------------------------
    #endregion
}
//=======================================================