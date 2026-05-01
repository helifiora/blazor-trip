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

    protected override void OnInitialized()
    {
        _model = NewTransactionModel.NewWithSharedAdded(People.Select(s => s.Id));
    }

    private async Task Submit(EditContext context)
    {
        var item = Transaction.Create(
            _model.Name,
            _model.Amount,
            _model.CategoryId,
            _model.PayerId,
            _model.SharedIds
        );

        await OnSubmit.InvokeAsync(item);

        _model = NewTransactionModel.NewWithSharedAdded(People.Select(s => s.Id));
        await InvokeAsync(StateHasChanged);
        await Task.Delay(TimeSpan.FromMilliseconds(100));        
        if (_nameRef?.Element is ElementReference e)
        {
            await e.FocusAsync();
        }

    }
}