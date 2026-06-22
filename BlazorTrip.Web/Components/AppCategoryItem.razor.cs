using BlazorTrip.Domain;
using BlazorTrip.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Components;

public partial class AppCategoryItem
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter] [EditorRequired] public Category Category { get; set; }
    
    [Parameter] public EventCallback<Category> OnCategoryChanged { get; set; }

    [Parameter] public EventCallback<Category> OnRemoved { get; set; }

    private SelectCategoryIcon? _iconRef;

    private ElementReference _editContainerRef;

    private bool _isEditFirst = true;
    private bool _isEdit = false;

    private string _name = string.Empty;

    private string _logo = string.Empty;

    private async Task Remove()
    {
        await OnRemoved.InvokeAsync(Category);
    }

    private async Task Edit()
    {
        _isEdit = true;
        _name = Category.Name;
        _logo = Category.Logo;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_isEdit && _isEditFirst && _iconRef is not null)
        {
            _isEditFirst = false;
            await FocusIcon();
        }
    }

    private async Task CloseEditOnFocusOut()
    {
        if (!_isEdit) return;

        await Task.Delay(100);

        if (!_isEdit) return;

        var isFocusInside = await JsRuntime.InvokeAsync<bool>("focusHelper.containsActiveElement", _editContainerRef);

        if (!isFocusInside)
        {
            CloseEdit();
            await InvokeAsync(StateHasChanged);
        }
    }

    private void CloseEdit()
    {
        _isEditFirst = true;
        _isEdit = false;
        _name = string.Empty;
        _logo = string.Empty;
    }

    private async Task KeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await OnCategoryChanged.InvokeAsync(Category with { Logo = _logo, Name = _name });
            CloseEdit();
        }

        if (e.Key == "Escape")
        {
            CloseEdit();
        }
    }

    private async Task FocusIcon()
    {
        if (_iconRef is null) return;

        await Task.Delay(TimeSpan.FromMilliseconds(100));

        if (!_isEdit || _iconRef is null) return;

        await _iconRef.FocusAsync();
    }

    private async Task Save()
    {
        await OnCategoryChanged.InvokeAsync(Category with { Logo = _logo, Name = _name });
        CloseEdit();
    }
}
