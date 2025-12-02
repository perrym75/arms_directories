using ArmsDirectories.Domain.Contract.Models.Directories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmsDirectories.DAL.PostgreSql.Configurations.Directories;

public class DirectoryConfiguration : IEntityTypeConfiguration<ArmsDirectory>
{
    public void Configure(EntityTypeBuilder<ArmsDirectory> builder)
    {
        builder.HasKey(p => p.Id);
        builder
            .Property(p => p.Name)
            .HasMaxLength(255);
        builder
            .Property(p => p.SystemName)
            .HasMaxLength(64);
        builder
            .Property(p => p.TableName)
            .HasMaxLength(64)
            .HasDefaultValueSql($"'table_' || nextval('{Schemas.Default}.table_name_seq')");
        builder.HasMany(p => p.ArmsAttributes).WithOne();
    }
}