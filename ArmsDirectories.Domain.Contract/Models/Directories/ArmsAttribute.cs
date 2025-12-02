using ArmsDirectories.Domain.Contract.Models.Base;

namespace ArmsDirectories.Domain.Contract.Models.Directories;

/// <summary>
/// Описание атрибута справочника
/// </summary>
public record ArmsAttribute : IEntity<long>, ISoftDeletableEntity, ICreaateUpdateTrackingEntity
{
    /// <inheritdoc />
    public long Id { get; init; }

    /// <summary>
    /// Описание справочника
    /// </summary>
    public required ArmsDirectory Directory { get; init; }

    public required string Name { get; set; }

    public required string SystemName { get; set; }

    public string ColumnName { get; init; } = null!;

    public required string DataType { get; set; }

    public int MaxLength { get; set; }

    public int Precision { get; set; }

    public int Scale { get; set; }

    public bool IsUnique { get; init; }

    public string? DefaultValue { get; set; }

    public ArmsDirectory? ReferenceDirectory { get; set; }

    public int DisplayOrder { get; set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
}
