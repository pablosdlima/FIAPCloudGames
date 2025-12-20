namespace FIAPCloudGames.Domain.Models;
//====================================================
public class Role
{
    #region Propriedades
    //------------------------------------------------
    public Guid IdRole { get; set; }
    //------------------------------------------------
    public string RoleName { get; set; }
    //------------------------------------------------
    public string? Description { get; set; }
    //------------------------------------------------
    #endregion

    #region Listas e Objetos (Relacionamentos)
    //--------------------------------------------------------
    public ICollection<UsuarioRole> Usuarios { get; set; }
    //------------------------------------------------
    #endregion
}
//====================================================