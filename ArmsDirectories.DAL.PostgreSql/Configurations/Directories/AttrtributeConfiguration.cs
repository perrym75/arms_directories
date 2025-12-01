using ArmsDirectories.Domain.Contract.Models.Directories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmsDirectories.DAL.PostgreSql.Configurations.Directories;

public class AttributeConfiguration : IEntityTypeConfiguration<ArmsAttribute>
{
    public void Configure(EntityTypeBuilder<ArmsAttribute> builder)
    {
        builder.HasKey(p => p.Id);
        builder
            .Property(p => p.Name)
            .HasMaxLength(255);
        builder
            .Property(p => p.SystemName)
            .HasMaxLength(64);
        builder
            .Property(p => p.ColumnName)
            .HasMaxLength(64)
            .HasDefaultValueSql($"'column_' || nextval('{Schemas.Default}.column_name_seq')");
        builder
            .Property(p => p.DataType)
            .HasMaxLength(50);
    }
}