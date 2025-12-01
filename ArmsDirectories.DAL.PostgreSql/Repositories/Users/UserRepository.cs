using ArmsDirectories.Domain.Contract.Models.Base;
using ArmsDirectories.DAL.PostgreSql.Repositories.Base;
using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Users;
using ArmsDirectories.Domain.Contract.Models.Users;

namespace ArmsDirectories.DAL.PostgreSql.Repositories.Users;

public class UserRepository : BaseRepository<EntityId<User>, User>, IUserRepository
{
    public UserRepository(MainApiDbContext context) : base(context)
    {
    }
}