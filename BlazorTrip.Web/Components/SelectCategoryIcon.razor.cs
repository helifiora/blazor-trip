using BlazorTrip.Web.WebApis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorTrip.Web.Components;

public partial class SelectCategoryIcon(PopoverApi popoverApi) : ComponentBase
{
    private readonly string _uniqueId = Guid.NewGuid().ToString("N");

    private ElementReference _popoverTarget;

    private ElementReference _buttonRef;

    [Parameter] [EditorRequired] public string Value { get; set; }

    [Parameter] [EditorRequired] public EventCallback<string> ValueChanged { get; set; }

    [Parameter] public string GroupName { get; set; } = $"{Guid.NewGuid()}-icon-selector-group";

    public async Task FocusAsync()
    {
        await _buttonRef.FocusAsync();
    }

    private async Task OnIconSelected(string selected)
    {
        Value = selected;
        await ValueChanged.InvokeAsync(Value);
    }

    public static readonly string[] AvailableIcons =
    [
        // Comida e Bebida
        "ph-hamburger",
        "ph-avocado",
        "ph-fish",
        "ph-beer-bottle",

        // Transporte e Viagem
        "ph-airplane-takeoff",
        "ph-car",
        "ph-signpost",

        // Construções e Moradia
        "ph-house-simple",
        "ph-building-apartment",

        // Compras e Entregas
        "ph-shopping-cart",
        "ph-package",
        "ph-barcode",

        // Objetos Diversos e Escritório
        "ph-address-book",
        "ph-article",
        "ph-book",
        "ph-baby-carriage",
        "ph-balloon",
        "ph-alarm",
        "ph-scan",
        "ph-bag",
        "ph-bread",
        "ph-camera",
        "ph-coin",
        "ph-database",
        "ph-warehouse"
    ];

    private async Task OnTogglePopover()
    {
        if (await popoverApi.IsOpen(_popoverTarget) && Value == string.Empty)
        {
            await OnIconSelected(AvailableIcons.First());
        }
    }

    private async Task OnKeyDown(KeyboardEventArgs obj)
    {
        if (obj.Key == "Tab")
        {
            await popoverApi.Close(_popoverTarget);
        }
    }
}