using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Dtos;

public class ContatosDtos
{
    public Guid IdContato { get; set; }
    public Guid UsuarioId { get; set; }
    public string Celular { get; set; }
    public string Email { get; set; }

    #region Relacionamentos
    public virtual UsuarioDtos? Usuario { get; set; }
    #endregion
}