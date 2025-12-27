namespace FIAPCloudGames.Domain.Models;

public class Contato
{
    public Contato(string celular, string email)
    {
        Id = Guid.NewGuid();
        Celular = celular;
        Email = email;
    }

    #region Propriedades

    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Celular { get; set; }
    public string Email { get; set; }

    #endregion

    #region Listas e Objetos (Relacionamentos)

    public virtual Usuario Usuario { get; set; }

    #endregion
}