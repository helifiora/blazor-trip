using BlazorTrip.Domain;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class AppCategoryItem
{
    [Parameter] [EditorRequired] public Category Category { get; set; }

    [Parameter] public EventCallback<Category> OnRemoved { get; set; }

    public async Task Remove()
    {
        await OnRemoved.InvokeAsync(Category);
    }
}