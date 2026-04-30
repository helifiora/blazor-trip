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

        var totalSpent = transactionRepository.Transactions
            .Where(s => s.PayerId == personId)
            .Select(s => s.Amount)
            .Sum();

        var sharesRelatedToMe = transactionRepository.Transactions
            .SelectMany(s => s.CalculateTransactionShares())
            .AsEnumerable()
            .Where(s => s.ToPayId == personId || s.ToReceiveId == personId)
            .OrderBy(s => s.ToPayId == personId ? 1 : -1)
            .ThenByDescending(s => s.ShareAmount)
            .ToList();

        var reportShare = sharesRelatedToMe.Select(s => new ReportShareDto(
            s.Transaction,
            personRepository.GetById(s.ToPayId)!,
            personRepository.GetById(s.ToReceiveId)!,
            s.ToReceiveId == personId ? s.ShareAmount : -s.ShareAmount,
            categoryRepository.GetById(s.Transaction.CategoryId)!
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

        return new ReportDto(person, totalSpent, balance, payers);
    }
}