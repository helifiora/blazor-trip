using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class UpdateTransactionCard : ComponentBase
{
    [Parameter] [EditorRequired] public IEnumerable<Person> People { get; set; }

    [Parameter] [EditorRequired] public IEnumerable<Category> Categories { get; set; }

    [Parameter] [EditorRequired] public TransactionDto Transaction { get; set; }

    [Parameter] public EventCallback<Transaction> OnSubmit { get; set; }

    [Parameter] public EventCallback OnClose { get; set; }

    private NewTransactionModel _model = new();

    protected override void OnInitialized()
    {
        _model = new NewTransactionModel
        {
            Amount = Transaction.Amount,
            CategoryId = Transaction.Category.Id,
            Name = Transaction.Name,
            PayerId = Transaction.Payer.Id,
            SharedIds = Transaction.Shared.Select(s => s.Id).ToHashSet()
        };
    }

    private async Task Submit()
    {
        var item = new Transaction(
            Transaction.Id,
            _model.Name,
            _model.Amount,
            _model.CategoryId,
            _model.PayerId,
            _model.SharedIds
        );

        await OnSubmit.InvokeAsync(item);
    }
}