using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Data.Mappings;
//=====================================================
public class GameMap : IEntityTypeConfiguration<Game>
{
    #region Interfaces
    //-------------------------------------------------------
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Game");
        builder.HasKey(primaryKey => primaryKey.IdGame);

        builder.Property(g => g.IdGame)
              .ValueGeneratedNever();

        builder.Property(g => g.Nome)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(g => g.Descricao)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(g => g.Genero)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(g => g.Desenvolvedor)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(g => g.DataRelease)
               .IsRequired();

        builder.Property(g => g.Preco)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(g => g.DataCriacao)
               .IsRequired();


        #region Foreign Key
        //---------------------------------------------------------------
        builder.HasMany(g => g.Biblioteca)
              .WithOne(b => b.Game)
              .HasForeignKey(b => b.GameId)
              .OnDelete(DeleteBehavior.Restrict);
        //---------------------------------------------------------------
        #endregion
    }
    //-------------------------------------------------------
    #endregion
}
//=====================================================
