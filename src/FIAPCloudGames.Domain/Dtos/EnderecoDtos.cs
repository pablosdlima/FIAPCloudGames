using FIAPCloudGames.Domain.Dtos;

namespace FIAPCloudGames.Application.Dtos;
//===================================================
public class EnderecoDtos
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid IdEndereco { get; set; }
    //--------------------------------------------------------
    public Guid UsuarioId { get; set; }
    //--------------------------------------------------------
    public string Rua { get; set; }
    //--------------------------------------------------------
    public string Numero { get; set; }
    //--------------------------------------------------------
    public string? Complemento { get; set; }
    //--------------------------------------------------------
    public string? Bairro { get; set; }
    //--------------------------------------------------------
    public string? Cidade { get; set; }
    //--------------------------------------------------------
    public string? Estado { get; set; }
    //--------------------------------------------------------
    public string? Cep { get; set; }
    //--------------------------------------------------------
    #endregion

    #region Relacionamentos
    public virtual UsuarioDtos? Usuario { get; set; }
    #endregion
}
//===================================================
