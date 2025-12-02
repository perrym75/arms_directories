using ArmsDirectories.Domain.Contract.Models.Base;

namespace ArmsDirectories.Domain.Contract.Models.Users;

using UserId = EntityId<User>;

public record User : IEntity<UserId>, ISoftDeletableEntity
{
    public UserId Id { get; init; } = UserId.NewId();
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public bool IsDeleted { get; set; }
};