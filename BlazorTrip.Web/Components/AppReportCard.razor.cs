using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public record PersonTransaction(decimal ShareAmount, Transaction Transaction);

public partial class AppReportCard
{
    [Parameter] [EditorRequired] public ReportDto Report { get; set; }

    private List<PersonTransaction> PersonTransactions => Report.TransactionShares
        .SelectMany(s => s.Shares)
        .Select(s => new PersonTransaction(s.SharedAmount, s.Transaction))
        .Where(s => s.Transaction.PayerId == Report.Person.Id)
        .Distinct()
        .ToList();

    private string GetAmountClass(decimal amount)
    {
        return amount switch
        {
            < 0 => "-negative",
            > 0 => "-positive",
            _ => "-muted"
        };
    }

    private string GetAmountStatus(decimal amount)
    {
        return amount switch
        {
            < 0 => "Pagar",
            > 0 => "Receber",
            _ => "Nada a restituir"
        };
    }
}