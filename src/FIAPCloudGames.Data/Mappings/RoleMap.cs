using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Data.Mappings;

public class RoleMap : IEntityTypeConfiguration<Role>
{
    #region Interfaces

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");
        builder.HasKey(primary => primary.Id);

        builder.Property(r => r.Id)
               .ValueGeneratedNever();

        builder.Property(r => r.RoleName)
               .IsRequired()
               .HasColumnType("nvarchar(max)");

        builder.Property(r => r.Description)
               .IsRequired(false)
               .HasColumnType("nvarchar(max)");

        #region Foreign Keys

        builder.HasMany(r => r.Usuarios)
             .WithOne(ur => ur.Role)
             .HasForeignKey(ur => ur.RoleId)
             .OnDelete(DeleteBehavior.Restrict);

        #endregion
    }
    #endregion
}