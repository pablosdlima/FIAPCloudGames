namespace FIAPCloudGames.Domain.Dtos;

public class UsuarioDtos
{
    #region Propriedades

    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Senha { get; set; }
    public bool Ativo { get; set; }
    public DateTimeOffset? DataCriacao { get; set; }
    public DateTimeOffset? DataAtualizacao { get; set; }

    #endregion
}