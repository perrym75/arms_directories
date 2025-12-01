namespace ArmsDirectories.Domain.Contract.Models.Base;

/// <summary>
/// Сущность
/// </summary>
/// <typeparam name="TEntityId">Тип идентификатора сущности</typeparam>
public interface IEntity<TEntityId>
    where TEntityId : struct
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    TEntityId Id { get; init; }
}