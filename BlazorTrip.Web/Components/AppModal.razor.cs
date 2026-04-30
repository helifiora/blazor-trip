using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Components;

public partial class AppModal(IJSRuntime jsRuntime) : ComponentBase
{
    private bool _lastVisible;

    private ElementReference _modalRef;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] [EditorRequired] public bool Visible { get; set; }

    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Visible && !_lastVisible)
        {
            _lastVisible = true;
            await jsRuntime.InvokeVoidAsync("dialogHelper.show", _modalRef);
        }
        else if (!Visible && _lastVisible)
        {
            _lastVisible = false;
            await jsRuntime.InvokeVoidAsync("dialogHelper.close", _modalRef);
        }
    }

    private async Task Close()
    {
        if (Visible)
        {
            Visible = false;
            await VisibleChanged.InvokeAsync(false);
        }
    }
}