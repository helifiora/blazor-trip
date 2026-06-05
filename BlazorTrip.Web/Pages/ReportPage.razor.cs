using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Facade;
using BlazorTrip.Web.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Pages;

public partial class ReportPage(
    CreateReportAction createReportAction,
    IPersonRepository personRepository,
    ITransactionRepository transactionRepository
) : ComponentBase, IDisposable, IRecipient<PersonImportedMessage>
{
    private bool _previewIsVisible;
    private TransactionDto? _previewTransaction;

    private Person? _selectedPerson;

    private List<Person> _people = [];

    private ReportDto? _report;

    protected override void OnInitialized()
    {
        _ = FirstLoad();
        transactionRepository.OnChange += UpdateUi;
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public void Dispose()
    {
        transactionRepository.OnChange -= UpdateUi;
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    private void UpdateUi()
    {
        InvokeAsync(StateHasChanged);
    }

    private void ClosePreview()
    {
        _previewTransaction = null;
        _previewIsVisible = false;
    }

    private void OpenPreview(TransactionDto transaction)
    {
        _previewTransaction = transaction;
        _previewIsVisible = true;
    }

    private async Task FirstLoad()
    {
        _people = await personRepository.GetMany();
        _selectedPerson = _people.FirstOrDefault();
        if (_selectedPerson is not null)
        {
            await GenerateReport(_selectedPerson);
        }
    }

    public void Receive(PersonImportedMessage message)
    {
        _ = FirstLoad();
    }

    private async Task GenerateReport(Person person)
    {
        _report = await createReportAction.Handle(person.Id);
    }

    private async Task OnPersonSelected(Person? person)
    {
        _selectedPerson = person;
        if (person is not null)
        {
            await GenerateReport(person);
        }
    }
}