using BlazorTrip.Domain;
using BlazorTrip.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorTrip.Web.Components;

public partial class PersonItem : ComponentBase
{
    private ElementReference? _elementRef;

    [Parameter] public required Person Person { get; set; }

    [Parameter] public EventCallback<Person> OnPersonChanged { get; set; }

    [Parameter] public EventCallback<Person> OnRemoved { get; set; }

    private bool _isEdit;

    private string _name = "";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_elementRef is not null && _isEdit)
        {
            await _elementRef.Value.FocusAsync();
        }
    }

    private void Edit()
    {
        _name = Person.Name;
        _isEdit = true;
    }

    private void CloseEdit()
    {
        _isEdit = false;
        _name = "";
    }

    private async Task Remove()
    {
        await OnRemoved.InvokeAsync(Person);
    }

    private async Task KeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await OnPersonChanged.InvokeAsync(Person with { Name = _name });
            CloseEdit();
        }

        if (e.Key == "Escape")
        {
            CloseEdit();
        }
    }
}