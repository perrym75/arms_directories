namespace ArmsDirectories.Domain.Contract.Models.Base;

/// <summary>
/// Представляет сущность, поддерживает отслеживание времени создания и последнего изменения
/// </summary>
public interface ICreaateUpdateTrackingEntity
{
    /// <summary>
    /// Дата и время создания сущности в UTC
    /// </summary>
    DateTimeOffset CreatedAtUtc { get; init; }

    /// <summary>
    /// Дата и время последнего изменения сущности в UTC
    /// </summary>
    DateTimeOffset UpdatedAtUtc { get; set; }
}