using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Pages;

public partial class TransactionPage(
    IPersonRepository personRepository,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository,
    IJSRuntime jsRuntime
) : ComponentBase, IDisposable
{
    private TransactionDto? _selected;

    private bool _isUpdateVisible;

    private ElementReference _popoverNew;

    private IJSObjectReference? _jsModule;


    private void UpdateUi() => InvokeAsync(StateHasChanged);

    protected override void OnInitialized()
    {
        transactionRepository.OnChange += UpdateUi;
        categoryRepository.OnChange += UpdateUi;
        categoryRepository.OnChange += UpdateUi;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/TransactionPage.razor.js");
        }
    }

    public void Dispose()
    {
        transactionRepository.OnChange -= UpdateUi;
        categoryRepository.OnChange -= UpdateUi;
        categoryRepository.OnChange -= UpdateUi;
    }

    public async Task Submit(Transaction transaction)
    {
        await transactionRepository.Create(transaction);
    }

    public async Task Update(Transaction transaction)
    {
        await transactionRepository.Update(transaction);
        _isUpdateVisible = false;
    }

    private void ToUpdate(TransactionDto dto)
    {
        _selected = dto;
        _isUpdateVisible = true;
    }

    private async Task Delete(TransactionDto dto)
    {
        await transactionRepository.Delete(dto.Id);
    }

    private async Task ClosePopover()
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("togglePopover", _popoverNew);
        }
    }
}