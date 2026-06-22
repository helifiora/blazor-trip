using System.Collections.ObjectModel;
using BlazorTrip.Application.Messages;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Domain.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Application.ViewModels;

public partial class PersonPageViewModel(IPersonRepository personRepository, IMessenger messenger)
    : BaseViewModel(messenger), IRecipient<CsvImportedMessage>, IRecipient<PeopleChangedMessage>
{
    public event Action? OnChange;

    [ObservableProperty] private string _name = string.Empty;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private ObservableCollection<Person> _data = [];

    [RelayCommand]
    private async Task AddPerson()
    {
        if (string.IsNullOrEmpty(Name)) return;
        await personRepository.CreateAsync(Person.Create(Name));
        Name = string.Empty;
        OnChange?.Invoke();
    }

    [RelayCommand]
    private async Task DeletePerson(Person person)
    {
        await personRepository.DeleteAsync(person.Id);
        OnChange?.Invoke();
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;
        var data = await personRepository.GetAllAsync();
        Data = new ObservableCollection<Person>(data);
        IsLoading = false;
    }

    [RelayCommand]
    private async Task UpdatePerson(Person person)
    {
        await personRepository.UpdateAsync(person);
        OnChange?.Invoke();
    }

    public void Receive(CsvImportedMessage message) => _ = LoadAsync();

    public void Receive(PeopleChangedMessage message) => _ = LoadAsync();
}