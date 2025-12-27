namespace FIAPCloudGames.Domain.Models;

public class UsuarioRole
{

    public UsuarioRole(int roleId)
    {
        Id = Guid.NewGuid();
        RoleId = roleId;
    }

    #region Propriedades

    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public int RoleId { get; set; }

    #endregion

    #region Listas e Objetos (Relacionamentos)

    public virtual Usuario Usuario { get; set; }

    public virtual Role Role { get; set; }

    #endregion
}
