using FIAPCloudGames.Domain.Dtos;

namespace FIAPCloudGames.Application.Dtos;
//====================================================
public class UsuarioPerfilDto
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid IdPerfil { get; set; }
    //--------------------------------------------------------
    public Guid UsuarioId { get; set; }
    //--------------------------------------------------------
    public Guid GameId { get; set; }
    //--------------------------------------------------------
    public string NomeCompleto { get; set; }
    //--------------------------------------------------------
    public decimal PrecoAquisicao { get; set; }
    //--------------------------------------------------------
    public DateTimeOffset? DataNascimento { get; set; }
    //--------------------------------------------------------
    #endregion

    #region Relacionamentos
    public virtual UsuarioDtos? Usuario { get; set; }
    #endregion
}
//====================================================
