using BlazorTrip.Application.Dto;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public enum AppReportCardTab
{
    Summary,
    Transactions,
    Balance,
}

public partial class AppReportCard
{
    [Parameter] [EditorRequired] public ReportDto Report { get; set; }

    [Parameter] public EventCallback<TransactionDto> OnDetailTransaction { get; set; }

    [Parameter] public AppReportCardTab Tab { get; set; } = AppReportCardTab.Summary;

    [Parameter] public EventCallback<AppReportCardTab> TabChanged { get; set; }

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

    private void OnSelectTab(AppReportCardTab tab)
    {
        Tab = tab;
        _ = TabChanged.InvokeAsync(tab);
    }

    private List<PersonTransactionCategory> TotalSpendingItems => Report.Transactions
        .GroupBy(s => s.Category)
        .Select(s =>
        {
            var categoryAmount = s.Select(q => q.Amount).Sum();
            return new PersonTransactionCategory(s.Key, categoryAmount, s.Count());
        })
        .OrderByDescending(s => s.CategoryAmount)
        .ToList();

    private List<PersonTransactionCategory> BalanceSpendingItems => Report.ReportPayers
        .SelectMany(s => s.SharesToPay)
        .Concat(Report.ReportPayers.SelectMany(s => s.SharesToReceive))
        .GroupBy(s => s.Transaction.Category)
        .Select(s =>
        {
            var categoryAmount = s.Select(q => q.ShareAmount).Sum();
            return new PersonTransactionCategory(s.Key, categoryAmount, s.Count());
        })
        .OrderByDescending(s => s.CategoryAmount)
        .ToList();
}