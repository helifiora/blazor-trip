using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;

namespace BlazorTrip.Web.Repositories;

public interface ITransactionRepository : IDisposable
{
    event Action? OnChange;
    IEnumerable<Transaction> Transactions { get; }
    IEnumerable<TransactionDto> TransactionDtos { get; }
    Transaction? GetById(Guid id);
    Task<Transaction> Create(Transaction transaction);
    Task Update(Transaction transaction);
    Task Delete(Guid id);
    Task Import(IEnumerable<Transaction> transactions);
    void NotifyStateChanged();
}