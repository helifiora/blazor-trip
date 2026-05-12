using BlazorTrip.Domain;
using BlazorTrip.Web.Repositories;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Pages;

public partial class CategoryPage(ICategoryRepository categoryRepo) : ComponentBase, IDisposable
{
    private Guid? _id = null;

    private string _name = "";

    private string _logo = "";

    private ElementReference _nameReference;

    public string IsEditCss => _id is null ? "" : "-edit";

    protected override void OnInitialized()
    {
        categoryRepo.OnChange += UpdateUi;
    }

    public void Dispose()
    {
        categoryRepo.OnChange -= UpdateUi;
    }

    private async Task AddCategory()
    {
        if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_logo)) return;
        if (_id is null)
        {
            await categoryRepo.Save(_name, _logo);
        }
        else
        {
            await categoryRepo.Update(new Category(_id.Value, _name, _logo));
        }

        _name = "";
        _logo = "";
        _id = null;
        await _nameReference.FocusAsync();
    }

    public async Task RemoveCategory(Category categoryPage)
    {
        await categoryRepo.Delete(categoryPage.Id);
    }

    private async Task EditCategory(Category category)
    {
        _id = category.Id;
        _name = category.Name;
        _logo = category.Logo;
        await _nameReference.FocusAsync();
    }

    private void UpdateUi() => InvokeAsync(StateHasChanged);

    private async Task CancelEditCategory()
    {
        _id = null;
        _name = "";
        _logo = "";
        await _nameReference.FocusAsync();
    }
}