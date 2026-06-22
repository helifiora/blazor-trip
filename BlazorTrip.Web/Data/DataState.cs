using BlazorTrip.Domain;
using BlazorTrip.Domain.Models;

namespace BlazorTrip.Web.Data;

public class DataState
{
    public DataSet<Guid, Person> People { get; } = new(c => c.Id);

    public DataSet<Guid, Category> Categories { get; } = new(c => c.Id);

    public DataSet<Guid, Transaction> Transactions { get; } = new(c => c.Id);

    public void Import(
        IEnumerable<Transaction> transactions,
        IEnumerable<Category> categories,
        IEnumerable<Person> people,
        Action onUpdate)
    {
        Categories.Set(categories);
        Transactions.Set(transactions);
        People.Set(people);
        onUpdate();
    }
}