using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorTrip.Web.Components;

public partial class NewTransactionCard : ComponentBase
{
    [Parameter] [EditorRequired] public IEnumerable<Person> People { get; set; }

    [Parameter] [EditorRequired] public IEnumerable<Category> Categories { get; set; }

    [Parameter] public EventCallback<Transaction> OnSubmit { get; set; }

    [Parameter] public EventCallback OnClose { get; set; }

    private NewTransactionModel _model = new();

    private InputText? _nameRef;

    public void Reset()
    {
    }

    private async Task Submit()
    {
        var item = Transaction.Create(
            _model.Name,
            _model.Amount,
            _model.CategoryId,
            _model.PayerId,
            _model.SharedIds);

        await OnSubmit.InvokeAsync(item);
        _model.Reset();
        if (_nameRef?.Element is ElementReference e)
        {
            await e.FocusAsync();
        }
    }
}