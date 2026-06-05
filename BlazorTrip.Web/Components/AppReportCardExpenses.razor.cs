using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public record PersonTransactionCategory(
    Category Category,
    decimal CategoryAmount,
    int Count
);

public partial class AppReportCardExpenses : ComponentBase
{
    [Parameter] [EditorRequired] public ReportDto Report { get; set; }

    [Parameter] public EventCallback<TransactionDto> OnDetailTransaction { get; set; }

    
    private List<ReportShareDto> PersonTransactions => Report.TransactionShares
        .SelectMany(s => s.Shares)
        .Where(s => s.Transaction.Payer.Id == Report.Person.Id)
        .DistinctBy(s => s.Transaction.Id)
        .OrderByDescending(s => s.Transaction.Amount)
        .ToList();
}