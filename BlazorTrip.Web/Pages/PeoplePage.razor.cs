using BlazorTrip.Domain;
using BlazorTrip.Web.Repositories;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Pages;

public partial class PeoplePage(IPersonRepository personRepo) : ComponentBase, IDisposable
{
    private ElementReference _nameInput;

    private string _name = "";

    private async Task AddNewPerson()
    {
        await personRepo.Create(_name);
        _name = "";
        await _nameInput.FocusAsync();
    }

    private async Task Remove(Person person)
    {
        await personRepo.Delete(person.Id);
    }

    private void StateChange()
    {
        InvokeAsync(StateHasChanged);
    }

    protected override void OnInitialized()
    {
        personRepo.OnChange += StateChange;
    }

    public void Dispose()
    {
        personRepo.OnChange -= StateChange;
    }
}