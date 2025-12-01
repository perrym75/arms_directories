using System.Linq.Expressions;
using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;
using ArmsDirectories.Domain.Contract.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ArmsDirectories.DAL.PostgreSql.Repositories.Base;

/// <summary>
/// Реализация <see cref="IRepository{TEntityId,TEntity}" /> 
/// </summary>
/// <typeparam name="TEntityId">
/// Тип идентификатора сущности
/// </typeparam>
/// <typeparam name="TEntity">
/// Тип сущности
/// </typeparam>
public class BaseRepository<TEntityId, TEntity> : IRepository<TEntityId, TEntity>
	where TEntityId : struct
	where TEntity : class, IEntity<TEntityId>
{
	private readonly MainApiDbContext _context;

	protected BaseRepository(MainApiDbContext context)
	{
		_context = context;
	}

	/// <inheritdoc />
	public async Task<TEntity?> GetOneAsync(TEntityId id,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		CancellationToken cancellationToken = default)
	{
		var query = _context.Set<TEntity>().AsNoTracking();

		if (include is not null)
		{
			query = include(query);
		}

		return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
	}

	/// <inheritdoc />
	public async Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>>? predicate = null,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		CancellationToken cancellationToken = default)
	{
		var query = _context.Set<TEntity>().AsNoTracking();

		if (include is not null)
		{
			query = include(query);
		}

		if (predicate is not null)
		{
			query = query.Where(predicate);
		}

		return await query.FirstOrDefaultAsync(cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? where = null,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
		CancellationToken cancellationToken = default)
	{
		var query = _context.Set<TEntity>().AsNoTracking();

		if (include is not null)
		{
			query = include(query);
		}

		if (where is not null)
		{
			query = query.Where(where);
		}

		if (orderBy is not null)
		{
			query = orderBy(query);
		}

		return await query.ToListAsync(cancellationToken);
	}

	/// <inheritdoc />
	public virtual async Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		await _context.AddAsync(entity, cancellationToken);

		return entity;
	}

	/// <inheritdoc />
	public virtual async Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		_context.Attach(entity);
		_context.Entry(entity).State = EntityState.Modified;

		return await Task.FromResult(entity);
	}

	/// <inheritdoc />
	public virtual async Task DeleteAsync(TEntity entity, bool ignoreSoftDeletion = false,
		CancellationToken cancellationToken = default)
	{
		// Если сущность подлежит мягкому удалению - помечаем её как удалённую
		// В противном случае удаляем по обычной схеме
		if (entity is ISoftDeletableEntity && !ignoreSoftDeletion)
		{
			await SoftDeleteAsync(entity, cancellationToken);
		}
		else
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				_context.Attach(entity);
			}

			_context.Remove(entity);
		}
	}

	/// <inheritdoc />
	public virtual async Task DeleteAsync(TEntityId id, CancellationToken cancellationToken = default)
	{
		var entity = await GetOneAsync(id, null, cancellationToken);
		if (entity is null)
		{
			return;
		}

		await DeleteAsync(entity);
	}

	/// <inheritdoc />
	public async Task<bool> IsEntityExistsAsync(Expression<Func<TEntity, bool>> filter,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<TEntity>()
			.AnyAsync(filter, cancellationToken);
	}

	/// <inheritdoc />
	public virtual void BeginTransaction()
	{
		_context.Database.BeginTransaction();
	}

	/// <inheritdoc />
	public virtual void CommitTransaction()
	{
		_context.Database.CommitTransaction();
	}

	/// <inheritdoc />
	public virtual void RollbackTransaction()
	{
		_context.Database.RollbackTransaction();
	}

	/// <inheritdoc />
	public virtual async Task BeginTransactionAsync()
	{
		await _context.Database.BeginTransactionAsync();
	}

	/// <inheritdoc />
	public virtual async Task CommitTransactionAsync()
	{
		await _context.Database.CommitTransactionAsync();
	}

	/// <inheritdoc />
	public virtual async Task RollbackTransactionAsync()
	{
		await _context.Database.RollbackTransactionAsync();
	}

	/// <summary>
	/// Загружает дочерние сущности, которые также необходимо "мягко" удалить при удалении текущей сущности.
	/// </summary>
	/// <param name="entity">
	/// Сущность, для которого необходимо загрузить дочерние сущности.
	/// </param>
	/// <returns>
	/// Асинхронная операция, результатом которой будет список дочерних сущностей.
	/// </returns>
	protected virtual Task<IEnumerable<ISoftDeletableEntity>> LoadChildSoftDeletableEntitiesAsync(TEntity entity,
		CancellationToken cancellationToken = default) =>
		Task.FromResult(Enumerable.Empty<ISoftDeletableEntity>());

	private async Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		// пробуем загрузить дочерние сущности, которые также необходимо "мягко" удалить при удалении текущей сущности.
		var childSoftDeletedEntities = await LoadChildSoftDeletableEntitiesAsync(entity, cancellationToken);

		// формируем общий список сущностей для "мягкого" удаления
		var softDeletableEntities = new[] { (ISoftDeletableEntity)entity }
			.Concat(childSoftDeletedEntities)
			.ToList();

		// фиксируем время удаления
		var deleteDateTimeUtc = DateTimeOffset.UtcNow;

		foreach (var softDeletableEntity in softDeletableEntities)
		{
			// помечаем сущность как удалённую
			softDeletableEntity.IsDeleted = true;

			if (softDeletableEntity is ICreaateUpdateTrackingEntity softDeletedWithDateEntity)
			{
				// указываем так же время удаления, если это необходимо
				softDeletedWithDateEntity.UpdatedAtUtc = deleteDateTimeUtc;
			}

			_context.Entry(softDeletableEntity).State = EntityState.Modified;
		}
	}
}