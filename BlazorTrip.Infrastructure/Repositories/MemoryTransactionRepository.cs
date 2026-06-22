using BlazorTrip.Application.Dto;
using BlazorTrip.Application.Messages;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Domain.Models;
using BlazorTrip.Infrastructure.InMemory;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Infrastructure.Repositories;

public class MemoryTransactionRepository(InMemoryDaoContext dao, IMessenger messenger) : ITransactionRepository
{
    public Task<List<TransactionDto>> GetAllAsync(string search)
    {
        var result = from t in dao.Transactions.Items
            join c in dao.Categories.Items on t.CategoryId equals c.Id
            join p in dao.People.Items on t.PayerId equals p.Id
            where (
                search == string.Empty
                || t.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                || p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                || c.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            )
            select new TransactionDto(
                t.Id,
                t.Name,
                t.Amount,
                c,
                p,
                t.SharedIds.Select(s => dao.People.Items.Single(x => x.Id == s))
            );

        return Task.FromResult(result.ToList());
    }

    public Task CreateAsync(Transaction transaction)
    {
        if (!PayerAndCategoryExists(transaction)) return Task.FromException(new Exception());
        dao.Transactions.Add(transaction);
        NotifyChange();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid transactionId)
    {
        if (dao.Transactions.Remove(transactionId)) NotifyChange();
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Transaction transaction)
    {
        if (!PayerAndCategoryExists(transaction)) return Task.FromException(new Exception());
        if (dao.Transactions.Update(transaction)) NotifyChange();
        return Task.CompletedTask;
    }

    public void NotifyChange() => messenger.Send(new TransactionsChangedMessage());

    private bool PayerAndCategoryExists(Transaction transaction)
    {
        return dao.People.Exists(transaction.PayerId) && dao.Categories.Exists(transaction.CategoryId);
    }
}