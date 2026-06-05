using BlazorTrip.Domain;
using BlazorTrip.Web.Data;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Web.Repositories;

public class DataPersonRepository(DataState data) : IPersonRepository
{
    public Task<List<Person>> GetMany()
    {
        return Task.FromResult(data.People.ToList());
    }

    public Task<Person?> GetById(Guid id)
    {
        return Task.FromResult(data.People.GetOrNull(id));
    }

    public Task Save(Person person)
    {
        if (data.People.GetOrNull(person.Id) is not null)
        {
            data.People.Update(person);
            WeakReferenceMessenger.Default.Send(new PersonUpdatedMessage(person));
        }
        else
        {
            data.People.Add(person);
            WeakReferenceMessenger.Default.Send(new PersonAddedMessage(person));
        }

        return Task.CompletedTask;
    }

    public Task Delete(Guid id)
    {
        if (data.People.Remove(id))
            WeakReferenceMessenger.Default.Send(new PersonDeletedMessage(id));

        return Task.CompletedTask;
    }

    public Task Import(IEnumerable<Person> people)
    {
        var peopleData = people.ToList();
        data.People.Set(peopleData);
        WeakReferenceMessenger.Default.Send(new PersonImportedMessage(peopleData));
        return Task.CompletedTask;
    }
}