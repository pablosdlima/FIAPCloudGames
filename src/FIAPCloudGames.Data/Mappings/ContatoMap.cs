using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Data.Mappings;

public class ContatoMap : IEntityTypeConfiguration<Contato>
{
    #region Interface

    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("Contato");
        builder.HasKey(primaryKey => primaryKey.Id);

        builder.Property(e => e.Id)
           .ValueGeneratedOnAdd();

        builder.Property(c => c.Celular)
            .IsRequired(true);

        builder.Property(c => c.Email)
            .IsRequired(true);

        #region Foreign Keys

        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.Contatos)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion
    }

    #endregion
}