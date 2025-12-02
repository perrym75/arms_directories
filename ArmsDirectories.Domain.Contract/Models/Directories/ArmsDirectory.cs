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
    public required string Name { get; init; }

    /// <summary>
    /// Описание справочника
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Системное наименование справочника (наименование сущности в REST API)
    /// </summary>
    public required string SystemName { get; init; }

    /// <summary>
    /// Имя таблицы в БД
    /// </summary>
    public string TableName { get; init; } = null!;

    /// <inheritdoc />
    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public required User CreatedBy { get; init; }

    public required User UpdatedBy { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    public IList<ArmsAttribute>? ArmsAttributes { get; set; } = [];
}
