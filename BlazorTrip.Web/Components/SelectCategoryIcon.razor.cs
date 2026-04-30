using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class SelectCategoryIcon : ComponentBase
{
    private readonly string _uniqueId = Guid.NewGuid().ToString("N");
    
    [Parameter] [EditorRequired] public string Value { get; set; }

    [Parameter] [EditorRequired] public EventCallback<string> ValueChanged { get; set; }

    [Parameter] public string GroupName { get; set; } = $"{Guid.NewGuid()}-icon-selector-group";

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
        "ph-balloon"
    ];
}