namespace FIAPCloudGames.Domain.Models;
//====================================================
public class UsuarioPerfil
{
    #region Propriedades
    //------------------------------------------------
    public Guid Id { get; set; }
    //------------------------------------------------
    public Guid UsuarioId { get; set; }
    //------------------------------------------------
    public string NomeCompleto { get; set; }
    //------------------------------------------------
    public DateTime DataNascimento { get; set; }
    //------------------------------------------------
    public string Pais { get; set; }
    //------------------------------------------------
    public string AvatarUrl { get; set; }
    //------------------------------------------------
    #endregion

    #region Listas e Objetos (Relacionamentos)
    //--------------------------------------------------------
    public Usuario Usuario { get; set; }
    //--------------------------------------------------------
    #endregion

}
//====================================================
