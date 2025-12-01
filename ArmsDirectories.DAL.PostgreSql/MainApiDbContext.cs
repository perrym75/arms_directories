using Microsoft.EntityFrameworkCore;

namespace ArmsDirectories.DAL.PostgreSql;

public class MainApiDbContext(DbContextOptions<MainApiDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Default);

        modelBuilder.HasSequence<int>("table_name_seq")
            .StartsAt(1)
            .IncrementsBy(1);
        modelBuilder.HasSequence<int>("column_name_seq")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainApiDbContext).Assembly);
    }
}