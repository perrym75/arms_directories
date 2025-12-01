namespace ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;

public interface IUnitOfWork
{
    /// <summary>
    /// Сохранить изменения
    /// </summary>
    Task SaveChangesAsync();
}