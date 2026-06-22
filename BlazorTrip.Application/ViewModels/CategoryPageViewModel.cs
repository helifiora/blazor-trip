using System.Collections.ObjectModel;
using BlazorTrip.Application.Messages;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Domain.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Application.ViewModels;

public partial class CategoryPageViewModel(ICategoryRepository categoryRepository, IMessenger messenger)
    : BaseViewModel(messenger), IRecipient<CategoriesChangedMessage>, IRecipient<CsvImportedMessage>
{
    public event Action? OnChange;

    [ObservableProperty] private string _name = string.Empty;

    [ObservableProperty] private string _logo = string.Empty;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private ObservableCollection<Category> _data = [];

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;
        var data = await categoryRepository.GetAllAsync();
        Data = new ObservableCollection<Category>(data);
        IsLoading = false;
    }

    [RelayCommand]
    private async Task AddCategoryAsync()
    {
        if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Logo)) return;
        await categoryRepository.CreateAsync(Category.Create(Name, Logo));
        Name = "";
        Logo = "";
        OnChange?.Invoke();
    }

    [RelayCommand]
    private async Task RemoveCategoryAsync(Category category)
    {
        await categoryRepository.DeleteAsync(category.Id);
        OnChange?.Invoke();
    }

    [RelayCommand]
    private async Task UpdateCategoryAsync(Category category)
    {
        await categoryRepository.UpdateAsync(category);
        OnChange?.Invoke();
    }

    public void Receive(CategoriesChangedMessage message) => _ = LoadAsync();

    public void Receive(CsvImportedMessage message) => _ = LoadAsync();
}