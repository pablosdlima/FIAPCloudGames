namespace FIAPCloudGames.Domain.Models;
//==========================================================
public class Usuario
{
    #region Propriedades
    //--------------------------------------------------------
    public Guid IdUsuario { get; set; }
    //--------------------------------------------------------
    public string Nome { get; set; }
    //--------------------------------------------------------
    public string Senha { get; set; }
    //--------------------------------------------------------
    public bool Ativo { get; set; }
    //--------------------------------------------------------
    public DateTime DataCriacao { get; set; }
    //--------------------------------------------------------
    public DateTime DataAtualizacao { get; set; }
    //--------------------------------------------------------
    #endregion

    #region Listas e Objetos (Relacionamentos)
    //--------------------------------------------------------
    public UsuarioPerfil Perfil { get;  set; }
    //--------------------------------------------------------
    public ICollection<UsuarioRole> UsuarioRoles { get;  set; }
    //--------------------------------------------------------
    public ICollection<UsuarioGameBiblioteca> Biblioteca { get;  set; }
    //--------------------------------------------------------
    public ICollection<Endereco> Enderecos { get;  set; }
    //--------------------------------------------------------
    public ICollection<Contato> Contatos { get;  set; }
    //--------------------------------------------------------
    #endregion
}
//==========================================================
