using BlazorTrip.Web.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class AppReportCard
{
    [Parameter] [EditorRequired] public ReportDto Report { get; set; }

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