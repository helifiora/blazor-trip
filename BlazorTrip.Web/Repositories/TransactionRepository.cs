using BlazorTrip.Domain;

namespace BlazorTrip.Web.Repositories;

public class TransactionRepository : ITransactionRepository
{
    public event Action? OnChange;

    private Dictionary<Guid, Transaction> _transactions = [];

    public IQueryable<Transaction> Transactions => _transactions
        .AsQueryable()
        .Select(s => s.Value);

    public Transaction? GetById(Guid id)
    {
        return _transactions.GetValueOrDefault(id);
    }

    public Task<Transaction> Create(Transaction transaction)
    {
        _transactions[transaction.Id] = transaction;
        OnChange?.Invoke();
        return Task.FromResult(transaction);
    }

    public Task<Transaction> Update(Transaction transaction)
    {
        _transactions[transaction.Id] = transaction;
        OnChange?.Invoke();
        return Task.FromResult(transaction);
    }

    public Task Delete(Guid id)
    {
        if (_transactions.Remove(id))
        {
            OnChange?.Invoke();
        }

        return Task.CompletedTask;
    }
}