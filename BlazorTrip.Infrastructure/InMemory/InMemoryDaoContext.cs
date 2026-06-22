using BlazorTrip.Domain.Models;

namespace BlazorTrip.Infrastructure.InMemory;

public class InMemoryDaoContext
{
    public InMemoryDao<Category, Guid> Categories = new(s => s.Id);
    public InMemoryDao<Transaction, Guid> Transactions = new(s => s.Id);
    public InMemoryDao<Person, Guid> People = new(s => s.Id);
}