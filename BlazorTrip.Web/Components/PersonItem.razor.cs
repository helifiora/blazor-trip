using BlazorTrip.Domain;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class PersonItem : ComponentBase
{
    [Parameter] public required Person Person { get; set; }

    [Parameter] public EventCallback<Person> OnRemoved { get; set; }

    private async Task Remove()
    {
        await OnRemoved.InvokeAsync(Person);
    }
}