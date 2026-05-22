using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Repositories;

namespace BlazorTrip.Web.Facade;

public class ReportFacade(
    IPersonRepository personRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository
)
{
    public ReportDto GenerateReportFrom(Guid personId)
    {
        var person = personRepository.GetById(personId)!;

        var sharesRelatedToMe = transactionRepository.Transactions
            .SelectMany(s => s.CreateTransactionShares())
            .AsEnumerable()
            .Where(s => s.ToPayId == personId || s.ToReceiveId == personId)
            .OrderBy(s => s.ToPayId == personId ? 1 : -1)
            .ThenByDescending(s => s.ShareAmount)
            .ToList();

        var reportShare = sharesRelatedToMe.Select(s => new ReportShareDto(
            ToTransactionDto(s.Transaction),
            personRepository.GetById(s.ToPayId)!,
            personRepository.GetById(s.ToReceiveId)!,
            s.ToReceiveId == personId ? s.ShareAmount : -s.ShareAmount
        )).ToList();

        var balance = reportShare.Select(s => s.SharedAmount).Sum();
        var payers = personRepository.People
            .Where(s => s.Id != personId)
            .Select(s =>
            {
                var shares = reportShare
                    .Where(q => q.PersonToPay.Id == s.Id || q.PersonToReceive.Id == s.Id)
                    .OrderByDescending(q => decimal.Abs(q.SharedAmount))
                    .ThenBy(q => q.PersonToReceive.Id == s.Id ? 1 : -1)
                    .ToList();

                var valueToPay = shares.Select(g => g.SharedAmount).Sum();
                return new ReportPayerDto(s, valueToPay, shares);
            })
            .OrderByDescending(s => decimal.Abs(s.ValueToPay))
            .ThenBy(s => s.PersonToPay.Id == personId ? -1 : 1);

        var totalSpent = CalculateTotalSpent(personId);
        return new ReportDto(person, totalSpent, balance, payers);
    }

    private decimal CalculateTotalSpent(Guid personId)
    {
        return transactionRepository.Transactions
            .Where(s => s.PayerId == personId)
            .Select(s => s.Amount)
            .Sum();
    }

    private TransactionDto ToTransactionDto(Transaction transaction)
    {
        return new TransactionDto(
            transaction.Id,
            transaction.Name,
            transaction.Amount,
            categoryRepository.GetById(transaction.CategoryId)!,
            personRepository.GetById(transaction.PayerId)!,
            transaction.SharedIds.Select(s => personRepository.GetById(s)!)
        );
    }
}