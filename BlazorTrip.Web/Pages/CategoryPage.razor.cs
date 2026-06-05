using BlazorTrip.Domain;
using BlazorTrip.Web.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Pages;

public partial class CategoryPage(ICategoryRepository categoryRepo) :
    ComponentBase,
    IDisposable,
    IRecipient<CategoryAddedMessage>,
    IRecipient<CategoryUpdatedMessage>,
    IRecipient<CategoryDeletedMessage>,
    IRecipient<CategoryImportedMessage>
{
    private string _name = "";

    private string _logo = "";

    private ElementReference _nameReference;

    private List<Category> _categories = [];

    protected override void OnInitialized()
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
        _ = Load();
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void Receive(CategoryAddedMessage message)
    {
        _categories.Add(message.Value);
        UpdateUi();
    }

    public void Receive(CategoryUpdatedMessage message)
    {
        var index = _categories.FindIndex(s => s.Id == message.Value.Id);
        if (index < 0) return;
        _categories[index] = message.Value;
        UpdateUi();
    }

    public void Receive(CategoryDeletedMessage message)
    {
        _categories.RemoveAll(c => c.Id == message.Value);
    }

    public void Receive(CategoryImportedMessage message)
    {
        _categories = message.Value;
        UpdateUi();
    }

    private async Task AddCategory()
    {
        if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_logo)) return;
        var category = Category.Create(_name, _logo);
        await categoryRepo.Save(category);
        _name = "";
        _logo = "";
        await _nameReference.FocusAsync();
    }

    public async Task RemoveCategory(Category categoryPage)
    {
        await categoryRepo.Delete(categoryPage.Id);
    }

    private void UpdateUi() => InvokeAsync(StateHasChanged);

    private async Task UpdateCategory(Category category)
    {
        await categoryRepo.Save(category);
        await _nameReference.FocusAsync();
    }

    private async Task Load()
    {
        _categories = await categoryRepo.GetMany();
    }
}