using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class AppSelectButtons<TItem> : ComponentBase
{
    private readonly string _uniqueId = Guid.NewGuid().ToString("N");

    [Parameter] [EditorRequired] public List<TItem> Items { get; set; }

    [Parameter] [EditorRequired] public Func<TItem, object> SelectText { get; set; }

    [Parameter] [EditorRequired] public TItem? Value { get; set; }

    [Parameter] public EventCallback<TItem?> ValueChanged { get; set; }

    private void OnValueChanged(TItem item)
    {
        Value = item;
        ValueChanged.InvokeAsync(item);
    }

    private bool IsChecked(TItem item) => EqualityComparer<TItem>.Default.Equals(Value, item);
}