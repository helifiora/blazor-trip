using BlazorTrip.Domain;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class AppCategoryItem
{
    [Parameter] [EditorRequired] public Category Category { get; set; }

    [Parameter] public bool HasActionButtons { get; set; } = true;
    
    [Parameter] public EventCallback<Category> OnRemoved { get; set; }
    
    [Parameter] public EventCallback<Category> OnEdited { get; set; }

    private async Task Remove()
    {
        await OnRemoved.InvokeAsync(Category);
    }
    
    private async Task Edit()
    {
        await OnEdited.InvokeAsync(Category);
    }
}