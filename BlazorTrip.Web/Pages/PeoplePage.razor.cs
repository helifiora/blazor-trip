using BlazorTrip.Domain;
using BlazorTrip.Web.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Pages;

public partial class PeoplePage(IPersonRepository personRepo) :
    ComponentBase,
    IDisposable,
    IRecipient<PersonAddedMessage>,
    IRecipient<PersonUpdatedMessage>,
    IRecipient<PersonDeletedMessage>,
    IRecipient<PersonImportedMessage>
{
    private ElementReference _nameInput;

    private string _name = "";

    private List<Person> _people = [];

    private async Task AddNewPerson()
    {
        await personRepo.Save(Person.Create(_name));
        _name = "";
        await _nameInput.FocusAsync();
    }

    private async Task Update(Person person)
    {
        await personRepo.Save(person);
        _ = FocusInput();
    }

    private async Task Remove(Person person)
    {
        await personRepo.Delete(person.Id);
        _ = FocusInput();
    }

    public void Receive(PersonImportedMessage message)
    {
        _people = message.Value;
        UpdateUi();
    }

    public void Receive(PersonAddedMessage message)
    {
        _people.Add(message.Value);
        UpdateUi();
    }

    public void Receive(PersonUpdatedMessage message)
    {
        var t = _people.FindIndex(person => person.Id == message.Value.Id);
        if (t >= 0)
        {
            _people[t] = message.Value;
            UpdateUi();
        }
        else
        {
            personRepo.GetMany().ContinueWith((task) =>
            {
                _people = task.Result.ToList();
                UpdateUi();
            });
        }
    }

    public void Receive(PersonDeletedMessage message)
    {
        var index = _people.FindIndex(s => s.Id == message.Value);
        if (index < 0) return;
        _people.RemoveAt(index);
        UpdateUi();
    }

    protected override async Task OnInitializedAsync()
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
        _people = await personRepo.GetMany();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await FocusInput();
        }
    }


    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    private void UpdateUi() => InvokeAsync(StateHasChanged);

    private async Task FocusInput()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        await _nameInput.FocusAsync();
    }
}