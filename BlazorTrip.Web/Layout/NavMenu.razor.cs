using BlazorTrip.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Layout;

public partial class NavMenu(
    CsvService csvService,
    IJSRuntime  jsRuntime
) : ComponentBase
{
    private async Task Upload(InputFileChangeEventArgs e)
    {
        var file = e.File;
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        await csvService.Import(reader);
    }

    private async Task Download()
    {
        var bytes = await csvService.Export();
        var base64String = Convert.ToBase64String(bytes);
        await jsRuntime.InvokeVoidAsync("downloadCsv", "minha-viagem.csv", base64String);
    }
}