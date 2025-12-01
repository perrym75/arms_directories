using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;
using ArmsDirectories.Domain.Contract.Models.Base;
using ArmsDirectories.Domain.Contract.Models.Users;

namespace ArmsDirectories.Domain.Contract.Interfaces.Repositories.Users;

public interface IUserRepository : IRepository<EntityId<User>, User>
{
}