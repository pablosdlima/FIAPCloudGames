namespace FIAPCloudGames.Domain.Models;

public class UsuarioPerfil
{
    public UsuarioPerfil(string nomeCompleto, DateTimeOffset? dataNascimento, string pais, string avatarUrl)
    {
        Id = Guid.NewGuid();
        NomeCompleto = nomeCompleto;
        DataNascimento = dataNascimento;
        Pais = pais;
        AvatarUrl = avatarUrl;
    }


    #region Propriedades

    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string NomeCompleto { get; set; }
    public DateTimeOffset? DataNascimento { get; set; }
    public string Pais { get; set; }
    public string AvatarUrl { get; set; }

    #endregion

    #region Listas e Objetos (Relacionamentos)

    public virtual Usuario Usuario { get; set; }

    #endregion
}