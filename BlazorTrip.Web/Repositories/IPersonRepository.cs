using BlazorTrip.Domain;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BlazorTrip.Web.Repositories;

public interface IPersonRepository
{
    public Task<List<Person>> GetMany();
    public Task<Person?> GetById(Guid id);
    public Task Save(Person person);
    public Task Delete(Guid id);
    public Task Import(IEnumerable<Person> people);
}

public class PersonImportedMessage(List<Person> people) : ValueChangedMessage<List<Person>>(people);

public class PersonAddedMessage(Person person) : ValueChangedMessage<Person>(person);

public class PersonUpdatedMessage(Person person) : ValueChangedMessage<Person>(person);

public class PersonDeletedMessage(Guid id) : ValueChangedMessage<Guid>(id);