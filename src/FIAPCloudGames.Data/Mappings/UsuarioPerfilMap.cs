using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Data.Mappings;
//====================================================
public class UsuarioPerfilMap : IEntityTypeConfiguration<UsuarioPerfil>
{
    #region Interfaces
    //-------------------------------------------------------------------------
    public void Configure(EntityTypeBuilder<UsuarioPerfil> builder)
    {
        builder.ToTable("UsuarioPerfil");
        builder.HasKey(primaryKey => primaryKey.IdUsuarioPerfil);

        builder.Property(up => up.IdUsuarioPerfil)
             .ValueGeneratedNever();

        builder.Property(up => up.NomeCompleto)
               .IsRequired(false)
               .HasColumnType("nvarchar(max)");

        builder.Property(up => up.DataNascimento)
               .IsRequired();

        builder.Property(up => up.Pais)
               .IsRequired(false)
               .HasColumnType("nvarchar(max)");

        builder.Property(up => up.AvatarUrl)
               .IsRequired(false)
               .HasColumnType("nvarchar(max)");

        #region Foreign Keys
        //---------------------------------------------------------------------
        builder.HasOne(up => up.Usuario)
               .WithOne(u => u.Perfil)
               .HasForeignKey<UsuarioPerfil>(up => up.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);
        //---------------------------------------------------------------------
        builder.HasIndex(up => up.UsuarioId) //1:1
              .IsUnique();
        //---------------------------------------------------------------------

        #endregion
    }
    //-------------------------------------------------------------------------
    #endregion
}
//====================================================
