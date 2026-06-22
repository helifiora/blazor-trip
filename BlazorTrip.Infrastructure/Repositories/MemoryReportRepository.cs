using BlazorTrip.Application.Dto;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Domain.Models;
using BlazorTrip.Infrastructure.InMemory;

namespace BlazorTrip.Infrastructure.Repositories;

public class MemoryReportRepository(InMemoryDaoContext dao) : IReportRepository
{
    public Task<ReportDto> GetPersonReportAsync(Guid personId)
    {
        var person = dao.People.Get(personId)!;

        var personTransactions = dao.Transactions.Items
            .Where(s => s.PayerId == personId)
            .Select(ToTransactionDto)
            .ToList();

        var shares = dao.Transactions.Items
            .Where(s => s.PayerId == personId || s.SharedIds.Contains(personId))
            .SelectMany(s => s.CreateTransactionShares())
            .Where(s => s.ToPayId == personId || s.ToReceiveId == personId)
            .AsEnumerable()
            .Select(ToTransactionShareDto)
            .ToList();

        var result = new Dictionary<Guid, RelatedPersonGrouped>();
        foreach (var share in shares)
        {
            if (share.ToPay.Id == personId)
            {
                var grouped = result.GetValueOrDefault(share.ToReceive.Id) ?? new RelatedPersonGrouped(share.ToReceive);
                grouped.SharesToPay.Add(share with { ShareAmount = -share.ShareAmount });
                result[share.ToReceive.Id] = grouped;
            }
            else if (share.ToReceive.Id == personId)
            {
                var grouped = result.GetValueOrDefault(share.ToPay.Id) ?? new RelatedPersonGrouped(share.ToPay);
                grouped.SharesToReceive.Add(share);
                result[share.ToPay.Id] = grouped;
            }
        }

        var relatedReports = result.Values.Select(item => item.ToReportPayerDto())
            .OrderByDescending(s => decimal.Abs(s.ValueToPay)).ToList();

        var balance = shares.Sum(s => (s.ToPay.Id == personId) ? -s.ShareAmount : s.ShareAmount);
        var personTotal = personTransactions.Sum(s => s.Amount);
        return Task.FromResult(new ReportDto(person, personTotal, balance, relatedReports, personTransactions));
    }

    private TransactionShareDto ToTransactionShareDto(TransactionShare share)
    {
        return new TransactionShareDto(
            Transaction: ToTransactionDto(share.Transaction),
            ToPay: dao.People.Get(share.ToPayId)!,
            ToReceive: dao.People.Get(share.ToReceiveId)!,
            ShareAmount: share.ShareAmount
        );
    }

    private TransactionDto ToTransactionDto(Transaction transaction)
    {
        return new TransactionDto(
            transaction.Id,
            transaction.Name,
            transaction.Amount,
            dao.Categories.Get(transaction.CategoryId)!,
            dao.People.Get(transaction.PayerId)!,
            transaction.SharedIds.Select(dao.People.Get).OfType<Person>()
        );
    }
}

class RelatedPersonGrouped(Person relatedPerson)
{
    public Person RelatedPerson { get; } = relatedPerson;
    public decimal SharesTotal => ToReceiveSharesTotal + ToPaySharesTotal;

    public List<TransactionShareDto> SharesToPay { get; } = [];
    public decimal ToPaySharesTotal => SharesToPay.Sum(s => s.ShareAmount);

    public List<TransactionShareDto> SharesToReceive { get; } = [];
    public decimal ToReceiveSharesTotal => SharesToReceive.Sum(s => s.ShareAmount);

    public ReportPayerDto ToReportPayerDto() => new(
        RelatedPerson,
        SharesTotal,
        SharesToPay.OrderByDescending(s => decimal.Abs(s.ShareAmount)).ToList(),
        ToPaySharesTotal,
        SharesToReceive.OrderByDescending(s => s.ShareAmount).ToList(),
        ToReceiveSharesTotal
    );
}