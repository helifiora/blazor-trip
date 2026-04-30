using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class PageHeader : ComponentBase
{
    [Parameter] [EditorRequired] public string Title { get; set; }

    [Parameter] [EditorRequired] public string SubTitle { get; set; }
    
    [Parameter] [EditorRequired] public string IconName { get; set; }
    
    public string IconClass => $"page-header__icon {IconName}";
}