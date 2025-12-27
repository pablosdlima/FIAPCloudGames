namespace FIAPCloudGames.Domain.Models;

public class UsuarioGameBiblioteca
{
    #region Propriedades

    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid GameId { get; set; }
    public string TipoAquisicao { get; set; }
    public decimal PrecoAquisicao { get; set; }
    public DateTimeOffset? DataAquisicao { get; set; }

    #endregion

    #region Listas e Objetos (Relacionamentos)

    public virtual Usuario Usuario { get; set; }
    public virtual Game Game { get; set; }

    #endregion
}

