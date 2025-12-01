using System.Linq.Expressions;
using ArmsDirectories.Domain.Contract.Models.Base;
using Microsoft.EntityFrameworkCore.Query;

namespace ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;

/// <summary>
/// Базовый интерфейс репозитория
/// </summary>
/// <typeparam name="TEntityId">
/// Тип идентификатора сущности
/// </typeparam>
/// <typeparam name="TEntity">
/// Тип сущности
/// </typeparam>
public interface IRepository<in TEntityId, TEntity>
    where TEntityId : struct
    where TEntity : class, IEntity<TEntityId>
{
    /// <summary>
    /// Получить экземпляр сущности по идентификатору с указанными связями
    /// </summary>
    /// <param name="id">
    /// Идентификатор экземпляра сущности
    /// </param>
    /// <param name="include">
    /// Связи
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns></returns>
    Task<TEntity?> GetOneAsync(TEntityId id,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить первый экземпляр сущности из выбранных по условию с указанными связями
    /// </summary>
    /// <param name="predicate">
    /// Условие выбора
    /// </param>
    /// <param name="include">
    /// Связи
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns></returns>
    Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить выбранные экземпляры сущности по условию с указанными связями
    /// </summary>
    /// <param name="where">
    /// Условие выбора
    /// </param>
    /// <param name="orderBy">
    /// Сортировка
    /// </param>
    /// <param name="include">
    /// Связи
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Создать новую сущность.
    /// </summary>
    /// <param name="entity">
    /// Новая сущность
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>
    /// Асинхронная операция, результатом которой будет созданная сущность.
    /// </returns>
    Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить сущность, существующую в системе.
    /// </summary>
    /// <param name="entity">
    /// Сущность.
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>
    /// Асинхронная операция, результатом которой будет обновлённая сущность.
    /// </returns>
    Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить сущность.
    /// </summary>
    /// <param name="entity">
    /// Сущность.
    /// </param>
    /// <param name="ignoreSoftDeletion">
    /// Игнорировать флаг о мягком удалении, если он есть, и удалить безвозвратно
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>
    /// Асинхронная операция, не возвращающая результат.
    /// </returns>
    Task DeleteAsync(TEntity entity, bool ignoreSoftDeletion = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить сущность, если она существует.
    /// </summary>
    /// <param name="id">
    /// Идентификатор сущности.
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>
    /// Асинхронная операция, не возвращающая результат.
    /// </returns>
    /// <remarks>
    /// Если сущности с указанным идентификатором не существует, ничего не произойдёт, и ошибки не будет.
    /// </remarks>
    Task DeleteAsync(TEntityId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существует ли сущность.
    /// </summary>
    /// <param name="filter">
    /// Фильтр для получения сущности.
    /// </param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
    /// <returns>
    /// Асинхронная операция, результатом которой будет <see langword="true"/>, если сущность существует;
    /// <see langword="false"/> в противном случае.
    /// </returns>
    Task<bool> IsEntityExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);


    /// <summary>
    /// Начать транзакцию
    /// </summary>
    void BeginTransaction()
    {
    }

    /// <summary>
    /// Завершить транзакцию
    /// </summary>
    void CommitTransaction()
    {
    }

    /// <summary>
    /// Откатить транзакцию
    /// </summary>
    void RollbackTransaction()
    {
    }

    /// <summary>
    /// Начать транзакцию
    /// </summary>
    Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Завершить транзакцию
    /// </summary>
    Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Откатить транзакцию
    /// </summary>
    Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }
}