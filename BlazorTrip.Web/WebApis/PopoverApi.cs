using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.WebApis;

public class PopoverApi(IJSRuntime jsRuntime)
{
    public async Task<bool> IsOpen(ElementReference el)
    {
        return await jsRuntime.InvokeAsync<bool>("popoverHelper.isOpen", el);
    }

    public async Task Open(ElementReference el)
    {
        await jsRuntime.InvokeVoidAsync("popoverHelper.show", el);
    }
    
    public async Task Close(ElementReference el)
    {
        await jsRuntime.InvokeVoidAsync("popoverHelper.close", el);
    }
    
    public async Task Toggle(ElementReference el)
    {
        await jsRuntime.InvokeVoidAsync("popoverHelper.toggle", el);
    }
}