using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Queries;
using BlazorTrip.Web.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Pages;

public partial class TransactionPage(
    IPersonRepository personRepository,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository,
    SelectTransactionDto selector,
    IJSRuntime jsRuntime
) : ComponentBase, IDisposable
{
    private IEnumerable<TransactionDto> _transactions = [];

    private TransactionDto? _selected;

    private bool _isUpdateVisible;

    private ElementReference _popoverNew;

    private IJSObjectReference? _jsModule;

    private void Reload()
    {
        _transactions = selector.GetAll().ToList();
        InvokeAsync(StateHasChanged);
    }

    protected override void OnInitialized()
    {
        personRepository.OnChange += Reload;
        categoryRepository.OnChange += Reload;
        transactionRepository.OnChange += Reload;
        Reload();
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
        personRepository.OnChange -= Reload;
        categoryRepository.OnChange -= Reload;
        transactionRepository.OnChange -= Reload;
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