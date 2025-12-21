namespace FIAPCloudGames.Application.Dtos;

public class UsuarioRoleDto
{
    #region Propriedades

    public Guid idUsuarioRole { get; set; }
    public Guid UsuarioId { get; set; }
    public int RoleId { get; set; }

    #endregion
}
