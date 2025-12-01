using ArmsDirectories.Domain.Contract.Models.Base;
using ArmsDirectories.Domain.Contract.Models.Users;

namespace ArmsDirectories.Domain.Contract.Models.Directories;

/// <summary>
/// Описание справочника
/// </summary>
public record ArmsDirectory : IEntity<long>, ISoftDeletableEntity, ICreaateUpdateTrackingEntity
{
    /// <inheritdoc />
    public long Id { get; init; }

    /// <summary>
    /// Наименование справочника
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Описание справочника
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Системное наименование справочника (наименование сущности в REST API)
    /// </summary>
    public string SystemName { get; init; } = string.Empty;

    /// <summary>
    /// Имя таблицы в БД
    /// </summary>
    public string TableName { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public User CreatedBy { get; init; }

    public User UpdatedBy { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
}
