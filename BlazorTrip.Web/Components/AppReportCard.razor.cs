using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
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
}