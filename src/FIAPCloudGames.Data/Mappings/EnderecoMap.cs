using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Data.Mappings;
//===================================================
public class EnderecoMap : IEntityTypeConfiguration<Endereco>
{
    #region Interfaces
    //-----------------------------------------------------------
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.ToTable("EnderecoTb");
        builder.HasKey(primaryKey => primaryKey.IdEndereco);

        builder.Property(e => e.IdEndereco)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Rua)
              .IsRequired(true)
              .HasColumnType("nvarchar(max)");

        builder.Property(e => e.Numero)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(e => e.Complemento)
               .IsRequired(false)
               .HasColumnType("nvarchar(max)");

        builder.Property(e => e.Bairro)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(e => e.Cidade)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(e => e.Estado)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(e => e.Cep)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        #region Foreign Key
        //------------------------------------------------------
        builder.HasOne(e => e.Usuario)
              .WithMany(u => u.Enderecos)
              .HasForeignKey(e => e.UsuarioId)
              .OnDelete(DeleteBehavior.Restrict);
        //------------------------------------------------------
        #endregion
    }
    //-----------------------------------------------------------
    #endregion


}
//===================================================