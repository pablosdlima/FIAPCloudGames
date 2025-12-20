using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Data.Mappings;
//===================================================
public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    #region Interfaces
    //-----------------------------------------------------------------
    #endregion
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("UsuarioTb");
        builder.HasKey(primaryKey => primaryKey.IdUsuario);

        builder.Property(u => u.IdUsuario)
              .ValueGeneratedNever();

        builder.Property(u => u.Nome)
               .IsRequired()
               .HasColumnType("nvarchar(max)");

        builder.Property(u => u.Senha)
               .IsRequired()
               .HasColumnType("nvarchar(max)");

        builder.Property(u => u.Ativo)
               .IsRequired();

        builder.Property(u => u.DataCriacao)
               .IsRequired();

        builder.Property(u => u.DataAtualizacao)
               .IsRequired();


        #region Foreign Keys
        //-----------------------------------------------------------

        //(1:1)
        builder.HasOne(u => u.Perfil)
               .WithOne(p => p.Usuario)
               .HasForeignKey<UsuarioPerfil>(p => p.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);

        // Usuario -> Roles (N:N via UsuarioRole)
        builder.HasMany(u => u.UsuarioRoles)
               .WithOne(ur => ur.Usuario)
               .HasForeignKey(ur => ur.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);

        // Usuario -> Biblioteca (N:N via UsuarioGameBiblioteca)
        builder.HasMany(u => u.Biblioteca)
               .WithOne(b => b.Usuario)
               .HasForeignKey(b => b.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);

        // Usuario -> Enderecos (1:N)
        builder.HasMany(u => u.Enderecos)
               .WithOne(e => e.Usuario)
               .HasForeignKey(e => e.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);

        // Usuario -> Contatos (1:N)
        builder.HasMany(u => u.Contatos)
               .WithOne(c => c.Usuario)
               .HasForeignKey(c => c.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
        //-----------------------------------------------------------
        #endregion
    }
    //-----------------------------------------------------------------
}
//===================================================