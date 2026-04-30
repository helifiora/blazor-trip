using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Components;

public partial class AppPopover(IJSRuntime jsRuntime)
{
    private bool _lastVisible;

    private ElementReference _popoverRef;

    [Parameter] public bool Visible { get; set; }
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private async Task Close()
    {
        if (Visible)
        {
            Visible = false;
            await VisibleChanged.InvokeAsync(false);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Visible && !_lastVisible)
        {
            _lastVisible = true;
            await jsRuntime.InvokeVoidAsync("popoverHelper.show", _popoverRef);
        }
        else if (!Visible && _lastVisible)
        {
            _lastVisible = false;
            await jsRuntime.InvokeVoidAsync("popoverHelper.close", _popoverRef);
        }
    }
}