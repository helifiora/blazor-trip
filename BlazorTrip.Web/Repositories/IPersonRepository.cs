using BlazorTrip.Domain;

namespace BlazorTrip.Web.Repositories;

public interface IPersonRepository
{
    public event Action? OnChange;
    public IEnumerable<Person> People { get; }
    public Person? GetById(Guid id);
    public Task<Person> Create(string name);
    public Task Delete(Guid id);
}