using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Repositories;

public interface IRepository<T> where T : IEntity
{
    Task<List<T>> GetManyAsync();
    Task<List<T>> GetManyAsync(Func<T, bool> predicate);
    Task<T?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByPredicateAsync(Func<T, bool> predicate);
    Task SaveAsync(T model);
    Task DeleteAsync(Guid id);
}