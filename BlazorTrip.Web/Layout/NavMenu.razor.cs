using BlazorTrip.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorTrip.Web.Layout;

public partial class NavMenu(
    CsvService csvService
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
        await csvService.Export();
    }
}