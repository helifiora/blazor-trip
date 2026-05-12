using System.Collections.ObjectModel;
using BlazorTrip.Domain;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BlazorTrip.Web.Repositories;

public interface IPersonRepository
{
    public event Action? OnChange;
    public IEnumerable<Person> People { get; }
    public Person? GetById(Guid id);
    public Task Save(string name);
    public Task Update(Person person);
    public Task Delete(Guid id);
    public Task Import(IEnumerable<Person> people);
    void NotifyChangedState();
}

public class PersonChangedMessage(List<Person> people) : ValueChangedMessage<List<Person>>(people);
