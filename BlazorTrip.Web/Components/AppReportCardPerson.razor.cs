using BlazorTrip.Application.Dto;
using Microsoft.AspNetCore.Components;
using TransactionDto = BlazorTrip.Application.Dto.TransactionDto;

namespace BlazorTrip.Web.Components;

public partial class AppReportCardPerson : ComponentBase
{
    [Parameter] [EditorRequired] public ReportPayerDto Payer { get; set; }

    [Parameter] public EventCallback<TransactionDto> OnDetailTransaction { get; set; }

    private bool _isOpen = false;
    
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

    private void ToggleOpen()
    {
        _isOpen = !_isOpen;
    }
}