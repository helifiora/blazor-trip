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
        await personRepo.Save(_name);
        _name = "";
        await _nameInput.FocusAsync();
    }

    private async Task Remove(Person person)
    {
        await personRepo.Delete(person.Id);
    }
    
    protected override void OnInitialized()
    {
        personRepo.OnChange += UpdateUi;
    }

    public void Dispose()
    {
        personRepo.OnChange -= UpdateUi;
    }

    private void UpdateUi() => InvokeAsync(StateHasChanged);
}