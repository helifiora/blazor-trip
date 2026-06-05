using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorTrip.Web.Pages;

public partial class TransactionPage(
    IPersonRepository personRepository,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository,
    IJSRuntime jsRuntime
) : ComponentBase, IDisposable, IRecipient<PersonImportedMessage>, IRecipient<CategoryImportedMessage>
{
    private TransactionDto? _selected;

    private bool _isUpdateVisible;

    private ElementReference _popoverNew;

    private IJSObjectReference? _jsModule;

    private List<Person> _people = [];

    private List<Category> _categories = [];

    private void UpdateUi() => InvokeAsync(StateHasChanged);

    protected override async Task OnInitializedAsync()
    {
        transactionRepository.OnChange += UpdateUi;
        _people = await personRepository.GetMany();
        _categories = await categoryRepository.GetMany();
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

    public void Receive(PersonImportedMessage message)
    {
        _people = message.Value;
        UpdateUi();
    }

    public void Receive(CategoryImportedMessage message)
    {
        _categories = message.Value;
        UpdateUi();
    }
}