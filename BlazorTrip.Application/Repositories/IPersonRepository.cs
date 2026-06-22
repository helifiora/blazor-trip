using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Repositories;

public interface IPersonRepository
{
    Task<List<Person>> GetAllAsync();
    Task CreateAsync(Person person);
    Task DeleteAsync(Guid personId);
    Task UpdateAsync(Person person);
}