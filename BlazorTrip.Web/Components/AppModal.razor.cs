using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Components;

public partial class AppModal(IJSRuntime jsRuntime) : ComponentBase, IAsyncDisposable
{
    private bool _lastVisible;

    private ElementReference _modalRef;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] [EditorRequired] public bool Visible { get; set; }

    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await jsRuntime.InvokeVoidAsync("dialogHelper.addBackdropClick", _modalRef);
        }

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

    public async ValueTask DisposeAsync()
    {
        await jsRuntime.InvokeVoidAsync("dialogHelper.removeBackdropClick", _modalRef);
    }
}