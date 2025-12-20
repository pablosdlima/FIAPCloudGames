using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Data.Data;
//=================================================================
public class Contexto : DbContext
{
    #region Construtor
    //------------------------------------------------------------------
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {

    }
    //------------------------------------------------------------------
    #endregion

    #region Override
    //------------------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder modelBuilder) //carrega as configs dos maps.
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Contexto).Assembly);
    }
    //------------------------------------------------------------------
    #endregion

    #region DbSets
    //------------------------------------------------------------------
    public DbSet<Contato> Contato { set; get; }
    //------------------------------------------------------------------
    public DbSet<Endereco> Endereco { set; get; }
    //------------------------------------------------------------------
    public DbSet<Game> Game { set; get; }
    //------------------------------------------------------------------
    public DbSet<Role> Role { set; get; }
    //------------------------------------------------------------------
    public DbSet<Usuario> Usuario { set; get; }
    //------------------------------------------------------------------
    public DbSet<UsuarioGameBiblioteca> UsuarioGameBiblioteca { set; get; }
    //------------------------------------------------------------------
    public DbSet<UsuarioPerfil> UsuarioPerfil { set; get; }
    //------------------------------------------------------------------
    public DbSet<UsuarioRole> UsuarioRole { set; get; }
    //------------------------------------------------------------------
    #endregion

}
//=================================================================
