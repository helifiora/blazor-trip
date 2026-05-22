using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Facade;
using BlazorTrip.Web.Repositories;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Pages;

public partial class ReportPage(
    ReportFacade reportFacade,
    IPersonRepository personRepository,
    ITransactionRepository transactionRepository
) : ComponentBase, IDisposable
{
    private bool _previewIsVisible;
    private TransactionDto? _previewTransaction;

    private List<ReportDto> _reports = [];

    protected override void OnInitialized()
    {
        _reports = personRepository.People
            .Select(p => reportFacade.GenerateReportFrom(p.Id))
            .ToList();

        transactionRepository.OnChange += UpdateUi;
    }

    public void Dispose()
    {
        transactionRepository.OnChange -= UpdateUi;
    }

    private void UpdateUi()
    {
        _reports = personRepository.People
            .Select(p => reportFacade.GenerateReportFrom(p.Id))
            .ToList();
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
}