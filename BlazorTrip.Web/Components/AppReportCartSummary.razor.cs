using BlazorTrip.Application.Dto;
using Microsoft.AspNetCore.Components;
using TransactionDto = BlazorTrip.Application.Dto.TransactionDto;

namespace BlazorTrip.Web.Components;

public partial class AppReportCartSummary : ComponentBase
{
    [Parameter] [EditorRequired] public ReportDto Report { get; set; }

    [Parameter] public EventCallback<TransactionDto> OnDetailTransaction { get; set; }


    private List<TransactionDto> PersonTransactions => Report.Transactions
        .OrderByDescending(s => s.Amount)
        .ToList();

    private List<PersonTransactionCategory> PersonCategories => PersonTransactions
        .GroupBy(s => s.Category)
        .Select(s =>
        {
            var categoryAmount = s.Select(q => q.Amount).Sum();
            return new PersonTransactionCategory(s.Key, categoryAmount, s.Count());
        })
        .OrderByDescending(s => s.CategoryAmount)
        .ToList();
}