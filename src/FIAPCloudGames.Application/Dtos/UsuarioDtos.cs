namespace FIAPCloudGames.Application.Dtos;
//==============================================
public class UsuarioDtos
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid IdUsuario { get; set; }
    //--------------------------------------------------------
    public string Nome { get; set; }
    //--------------------------------------------------------
    public string Senha { get; set; }
    //--------------------------------------------------------
    public bool Ativo { get; set; }
    //--------------------------------------------------------
    public DateTime DataCriacao { get; set; }
    //--------------------------------------------------------
    public DateTime DataAtualizacao { get; set; }
    //--------------------------------------------------------
    #endregion
}
//==============================================
