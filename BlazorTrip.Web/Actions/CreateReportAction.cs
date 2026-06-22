using BlazorTrip.Domain.Models;
using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Repositories;

namespace BlazorTrip.Web.Actions;

public class CreateReportAction(
    IPersonRepository personRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository
)
{
    public async Task<ReportDto> Handle(Guid personId)
    {
        var people = await personRepository.GetMany();
        var categories = await categoryRepository.GetMany();
        var peopleDict = people.ToDictionary(x => x.Id);
        var categoriesDict = categories.ToDictionary(x => x.Id);

        var person = peopleDict[personId];

        var sharesRelatedToMe = transactionRepository.Transactions
            .SelectMany(s => s.CreateTransactionShares())
            .AsEnumerable()
            .Where(s => s.ToPayId == personId || s.ToReceiveId == personId)
            .OrderBy(s => s.ToPayId == personId ? 1 : -1)
            .ThenByDescending(s => s.ShareAmount)
            .ToList();

        var reportShare = sharesRelatedToMe.Select(s => new ReportShareDto(
            ToTransactionDto(s.Transaction, peopleDict, categoriesDict),
            peopleDict[s.ToPayId],
            peopleDict[s.ToReceiveId],
            s.ToReceiveId == personId ? s.ShareAmount : -s.ShareAmount
        )).ToList();

        var balance = reportShare.Select(s => s.SharedAmount).Sum();
        var payers = people.Where(s => s.Id != personId)
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

    private TransactionDto ToTransactionDto(Transaction transaction, Dictionary<Guid, Person> people,
        Dictionary<Guid, Category> categories)
    {
        return new TransactionDto(
            transaction.Id,
            transaction.Name,
            transaction.Amount,
            categories[transaction.CategoryId],
            people[transaction.PayerId],
            transaction.SharedIds.Select(id => people[id])
        );
    }
}