using BlazorTrip.Domain;

namespace BlazorTrip.Web.Repositories;

public class PersonRepository : IPersonRepository
{
    private Dictionary<Guid, Person> _data = [];

    public event Action? OnChange;

    public IEnumerable<Person> People => _data.Select(s => s.Value);

    public PersonRepository()
    {
        // var p = Person.Create("Helielton Fioramonte");
        // _data[p.Id] = p;
        // p = Person.Create("Gabriele Lima Campos");
        // _data[p.Id] = p;
        // p = Person.Create("Cláudio Fioramonte");
        // _data[p.Id] = p;
        // p = Person.Create("Graça Fioramonte");
        // _data[p.Id] = p;
    }

    public Person? GetById(Guid id)
    {
        return _data.GetValueOrDefault(id);
    }

    public Task<Person> Create(string name)
    {
        var person = Person.Create(name);
        _data[person.Id] = person;
        OnChange?.Invoke();
        return Task.FromResult(person);
    }

    public Task Delete(Guid id)
    {
        if (_data.Remove(id))
        {
            OnChange?.Invoke();
        }

        return Task.CompletedTask;
    }

    public Task Import(IEnumerable<Person> people)
    {
        _data = people.ToDictionary(s => s.Id);
        OnChange?.Invoke();
        return Task.CompletedTask;
    }
}