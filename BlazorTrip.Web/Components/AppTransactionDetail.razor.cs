using BlazorTrip.Application.Dto;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class AppTransactionDetail : ComponentBase
{
    [Parameter] [EditorRequired] public TransactionDto Transaction { get; set; }

    [Parameter] public EventCallback OnClose { get; set; }

    private decimal SharedAmount => Transaction.Amount / Transaction.Shared.Count();
    

    private string SharedTitle()
    {
        var isInTransaction = Transaction.Shared.Any(s => s.Id == Transaction.Payer.Id);
        if (isInTransaction)
        {
            return Transaction.Shared.Count() switch
            {
                1 => "Pago para mim",
                _ => "Dividido entre"
            };
        }

        return "Pago para";
    }
}