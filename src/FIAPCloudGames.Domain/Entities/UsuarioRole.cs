namespace FIAPCloudGames.Domain.Models;
//===============================================================
public class UsuarioRole
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid IdUsuarioRole { get; set; }
    //--------------------------------------------------------
    public Guid UsuarioId { get; set; }
    //--------------------------------------------------------
    public Guid RoleId { get; set; }
    //--------------------------------------------------------
    #endregion

    #region Listas e Objetos (Relacionamentos)
    //--------------------------------------------------------
    public Usuario Usuario { get; set; }
    //--------------------------------------------------------
    public Role Role { get; set; }
    //--------------------------------------------------------
    #endregion
}
//===============================================================
