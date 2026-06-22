using BlazorTrip.Application.Messages;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Domain.Models;
using BlazorTrip.Infrastructure.InMemory;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Infrastructure.Repositories;

public class MemoryPersonRepository(InMemoryDaoContext dao, IMessenger messenger) : IPersonRepository
{
    public Task<List<Person>> GetAllAsync()
    {
        var result = dao.People.Items
            .OrderBy(s => s.Name)
            .ToList();

        return Task.FromResult(result);
    }

    public Task CreateAsync(Person person)
    {
        if (dao.People.Add(person)) NotifyChange();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid personId)
    {
        if (HasTransactionsWith(personId)) return Task.FromException(new Exception());
        if (dao.People.Remove(personId)) NotifyChange();
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Person person)
    {
        if (dao.People.Update(person)) NotifyChange();
        return Task.CompletedTask;
    }

    public Task<bool> ExistsManyAsync(params Guid[] ids)
    {
        var result = dao.People.Exists(ids);
        return Task.FromResult(result);
    }

    private bool HasTransactionsWith(Guid personId)
    {
        return dao.Transactions.GetWhere(s => s.PayerId == personId) != null;
    }


    private void NotifyChange() => messenger.Send(new PeopleChangedMessage());
}