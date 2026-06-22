using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task CreateAsync(Category category);
    Task DeleteAsync(Guid categoryId);
    Task UpdateAsync(Category category);
}