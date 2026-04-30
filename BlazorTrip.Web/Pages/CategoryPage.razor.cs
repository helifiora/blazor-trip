using BlazorTrip.Domain;
using BlazorTrip.Web.Repositories;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Pages;

public partial class CategoryPage(ICategoryRepository categoryRepo) : ComponentBase, IDisposable
{
    private string _name = "";

    private string _logo = "";

    private ElementReference _nameReference;

    protected override void OnInitialized()
    {
        categoryRepo.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        categoryRepo.OnChange -= StateHasChanged;
    }

    private async Task AddCategory()
    {
        if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_logo)) return;
        await categoryRepo.Create(_name, _logo);
        _name = "";
        _logo = "";
        await _nameReference.FocusAsync();
    }

    public async Task RemoveCategory(Category categoryPage)
    {
        await categoryRepo.Delete(categoryPage.Id);
    }
}