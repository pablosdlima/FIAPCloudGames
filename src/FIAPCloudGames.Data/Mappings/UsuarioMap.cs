using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Data.Mappings;

public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .IsRequired()
               .ValueGeneratedNever();

        builder.Property(u => u.Nome)
               .IsRequired()
               .HasMaxLength(200)
               .HasColumnType("nvarchar(200)");

        builder.HasIndex(u => u.Nome)
               .IsUnique()
               .HasDatabaseName("IX_Usuario_Nome_Unique");

        builder.Property(u => u.Senha)
               .IsRequired()
               .HasColumnType("nvarchar(max)");

        builder.Property(u => u.Ativo)
               .IsRequired();

        builder.Property(u => u.DataCriacao)
               .IsRequired(false);

        builder.Property(u => u.DataAtualizacao)
               .IsRequired(false);


        #region Foreign Keys        

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
               .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}