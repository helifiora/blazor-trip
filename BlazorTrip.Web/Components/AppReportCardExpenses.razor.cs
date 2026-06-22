using BlazorTrip.Application.Dto;
using BlazorTrip.Domain.Models;
using Microsoft.AspNetCore.Components;
using TransactionDto = BlazorTrip.Application.Dto.TransactionDto;

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

    private List<TransactionDto> PersonTransactions => Report.Transactions
        .OrderByDescending(s => s.Amount)
        .ToList();

    private decimal SharedAmount(TransactionDto transaction) => transaction.Amount / transaction.Shared.Count();
}