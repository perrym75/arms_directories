using ArmsDirectories.Domain.Contract.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmsDirectories.DAL.PostgreSql.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);
        builder
            .Property(p => p.Id)
            .HasConversion(v => v.Value, v => new(v));
        builder
            .Property(p => p.Email)
            .HasMaxLength(256);
        builder
            .Property(p => p.Name)
            .HasMaxLength(128);
        builder
            .Property(p => p.Surname)
            .HasMaxLength(128);
    }
}