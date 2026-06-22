using BlazorTrip.Application.Dto;
using BlazorTrip.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorTrip.Web.Components;

public partial class NewTransactionCard : ComponentBase
{
    [Parameter] [EditorRequired] public IEnumerable<Person> People { get; set; }

    [Parameter] [EditorRequired] public IEnumerable<Category> Categories { get; set; }

    [Parameter] public EventCallback<CreateTransactionDto> OnSubmit { get; set; }

    [Parameter] public EventCallback OnClose { get; set; }

    private CreateTransactionDto _model = new();

    private InputText? _nameRef;

    protected override void OnInitialized()
    {
        _model = CreateTransactionDto.NewWithSharedAdded(People.Select(s => s.Id));
    }

    protected override void OnParametersSet()
    {
        _model = CreateTransactionDto.NewWithSharedAdded(People.Select(s => s.Id));
    }

    private async Task Submit(EditContext context)
    {
        await OnSubmit.InvokeAsync(_model);
        _model = CreateTransactionDto.NewWithSharedAdded(People.Select(s => s.Id));
        await InvokeAsync(StateHasChanged);
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        if (_nameRef?.Element is { } e)
        {
            await e.FocusAsync();
        }
    }
}