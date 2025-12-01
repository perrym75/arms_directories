using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;

namespace ArmsDirectories.DAL.PostgreSql.Repositories.Base;

public class BaseUnitOfWork : IUnitOfWork
{
    private readonly MainApiDbContext _context;

    public BaseUnitOfWork(MainApiDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}