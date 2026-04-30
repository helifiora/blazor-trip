using BlazorTrip.Domain;

namespace BlazorTrip.Web.Repositories;

public interface ITransactionRepository
{
    public event Action? OnChange;

    public IQueryable<Transaction> Transactions { get; }

    public Transaction? GetById(Guid id);

    public Task<Transaction> Create(Transaction transaction);
    public Task<Transaction> Update(Transaction transaction);

    public Task Delete(Guid id);
}