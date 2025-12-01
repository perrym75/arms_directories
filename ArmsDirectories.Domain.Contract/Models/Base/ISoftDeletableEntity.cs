namespace ArmsDirectories.Domain.Contract.Models.Base;

/// <summary>
/// Представляет сущность, которая поддерживает "мягкое" удаление.
/// </summary>
public interface ISoftDeletableEntity
{
    /// <summary>
    /// Признак того, что сущность удалена
    /// </summary>
    bool IsDeleted { get; set; }
}