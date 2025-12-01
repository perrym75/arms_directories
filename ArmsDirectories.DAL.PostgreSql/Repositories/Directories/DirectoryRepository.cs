using ArmsDirectories.DAL.PostgreSql.Repositories.Base;
using ArmsDirectories.Domain.Contract.Models.Directories;

namespace ArmsDirectories.DAL.PostgreSql.Repositories.Directories;

public class DirectoryRepository : BaseRepository<long, ArmsDirectory>
{
    public DirectoryRepository(MainApiDbContext context) : base(context)
    {
    }
}