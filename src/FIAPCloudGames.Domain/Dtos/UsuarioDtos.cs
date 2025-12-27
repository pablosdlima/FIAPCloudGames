using FIAPCloudGames.Domain.Enums;

namespace FIAPCloudGames.Domain.Dtos;

public class UsuarioDtos
{
    #region Propriedades

    public string Nome { get; set; }
    public string Senha { get; set; }
    public string Celular { get; set; }
    public string Email { get; set; }
    public TipoUsuario TipoUsuario { get; set; }

    #endregion
}