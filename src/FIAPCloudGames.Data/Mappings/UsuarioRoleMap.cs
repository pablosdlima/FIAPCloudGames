using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Data.Mappings;

public class UsuarioRoleMap : IEntityTypeConfiguration<UsuarioRole>
{
    #region Interfaces

    public void Configure(EntityTypeBuilder<UsuarioRole> builder)
    {
        builder.ToTable("UsuarioRole");

        builder.HasKey(primaryKey => primaryKey.Id);

        builder.Property(ur => ur.Id)
               .ValueGeneratedNever();

        builder.Property(ur => ur.UsuarioId)
               .IsRequired();

        builder.Property(ur => ur.RoleId)
               .IsRequired();

        #region Foreign Key

        // Usuario (1) -> UsuarioRole (N)
        builder.HasOne(ur => ur.Usuario)
               .WithMany(u => u.UsuarioRoles)
               .HasForeignKey(ur => ur.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);

        // Role (1) -> UsuarioRole (N)
        builder.HasOne(ur => ur.Role)
               .WithMany(r => r.Usuarios)
               .HasForeignKey(ur => ur.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ur => new { ur.UsuarioId, ur.RoleId })
             .IsUnique();

        #endregion
    }

    #endregion
}