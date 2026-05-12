using BlazorTrip.Domain;
using BlazorTrip.Web.Data;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Web.Repositories;

public class DataPersonRepository(DataState data) : IPersonRepository
{
    public event Action? OnChange;

    public IEnumerable<Person> People => data.People;

    public Person? GetById(Guid id) => data.People.GetOrNull(id);

    public Task Save(string name)
    {
        data.People.Add(Person.Create(name));
        NotifyChangedState();
        return Task.CompletedTask;
    }

    public Task Delete(Guid id)
    {
        if (data.People.Remove(id))
            NotifyChangedState();
        return Task.CompletedTask;
    }

    public Task Update(Person person)
    {
        data.People.Update(person);
        NotifyChangedState();
        return Task.CompletedTask;
    }

    public Task Import(IEnumerable<Person> people)
    {
        data.People.Set(people);
        NotifyChangedState();
        return Task.CompletedTask;
    }

    public void NotifyChangedState()
    {
        OnChange?.Invoke();
        WeakReferenceMessenger.Default.Send(new PersonChangedMessage(People.ToList()));
    }
}