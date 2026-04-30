using System.Data;
using BlazorTrip.Domain;

namespace BlazorTrip.Application;

public class PersonTransactionShare
{
    public required List<ShareReport> TransactionShares { get; init; }
}

public class ShareReport(Person paidBy, Person receivedBy)
{
    public Person PaidBy { get; } = paidBy;

    public Person ReceivedBy { get; } = receivedBy;

    public List<TransactionShare> Shares { get; } = [];

    // public decimal TotalAmount => Shares.Aggregate();

    public void AddShare(TransactionShare share)
    {
        Shares.Add(share);
    }
}

public class GenerateReport
{
    public PersonTransactionShare Generate(IEnumerable<Person> people, IEnumerable<Transaction> transactions)
    {
        var shares = transactions.SelectMany(s => s.CalculateTransactionShares());
        //
        // var peopleReport = people.ToDictionary(s => s, _ => new Dictionary<Person, List<TransactionShare>>());
        // foreach (var share in shares)
        // {
        //     AddTransactionShare(peopleReport, share.PaidBy, share.ReceivedBy, share);
        //     AddTransactionShare(peopleReport, share.ReceivedBy, share.PaidBy, share);
        // }

        throw new SyntaxErrorException();
    }

    private void AddTransactionShare(Dictionary<Person, Dictionary<Person, List<TransactionShare>>> report,
        Person paidBy,
        Person receivedBy, TransactionShare share)
    {
        if (!report.TryGetValue(receivedBy, out var receivedReport)) return;
        if (receivedReport.TryGetValue(paidBy, out var paidByReport))
        {
            paidByReport.Add(share);
        }
        else
        {
            receivedReport.Add(paidBy, [share]);
        }
    }
}