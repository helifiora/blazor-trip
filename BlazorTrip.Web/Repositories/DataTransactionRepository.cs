using BlazorTrip.Domain;
using BlazorTrip.Web.Data;
using BlazorTrip.Web.Dtos;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Web.Repositories;

public class DataTransactionRepository : ITransactionRepository
{
    private DataState _data;

    public event Action? OnChange;

    private List<TransactionDto> _cache = [];

    public IEnumerable<Transaction> Transactions => _data.Transactions;

    public IEnumerable<TransactionDto> TransactionDtos => _cache;

    public DataTransactionRepository(DataState data)
    {
        _data = data;
        UpdateTransformDtoCache();
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public Transaction? GetById(Guid id) => _data.Transactions.GetOrNull(id);

    public Task<Transaction> Create(Transaction transaction)
    {
        _data.Transactions.Add(transaction);
        NotifyStateChanged();
        return Task.FromResult(transaction);
    }

    public Task Update(Transaction transaction)
    {
        _data.Transactions.Add(transaction);
        NotifyStateChanged();
        return Task.FromResult(transaction);
    }

    public Task Delete(Guid id)
    {
        if (_data.Transactions.Remove(id))
            NotifyStateChanged();

        return Task.CompletedTask;
    }

    public Task Import(IEnumerable<Transaction> transactions)
    {
        _data.Transactions.Set(transactions);
        NotifyStateChanged();
        return Task.CompletedTask;
    }

    public void NotifyStateChanged()
    {
        OnChange?.Invoke();
        UpdateTransformDtoCache();
    }
    
    private void UpdateTransformDtoCache()
    {
        _cache = _data.Transactions
            .Join(_data.People, s => s.PayerId, s => s.Id,
                (transaction, person) => new { Transaction = transaction, Person = person })
            .Join(_data.Categories, s => s.Transaction.CategoryId, s => s.Id, (record, category) => new TransactionDto(
                record.Transaction.Id,
                record.Transaction.Name,
                record.Transaction.Amount,
                category,
                record.Person,
                record.Transaction.SharedIds.Select(_data.People.Get)
            )).ToList();
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}