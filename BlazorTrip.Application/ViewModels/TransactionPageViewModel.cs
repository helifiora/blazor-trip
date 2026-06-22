using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using BlazorTrip.Application.Dto;
using BlazorTrip.Application.Messages;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Domain.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Application.ViewModels;

public partial class TransactionPageViewModel :
    BaseViewModel,
    IRecipient<CsvImportedMessage>,
    IRecipient<TransactionsChangedMessage>
{
    private readonly IPersonRepository _personRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    private IDisposable _searchDisposable;
    private Subject<string> _searchSubject;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsUpdateVisible))]
    private TransactionDto? _selectedTransaction;

    [ObservableProperty] bool _isLoading;

    [ObservableProperty] private ObservableCollection<Person> _people = [];

    [ObservableProperty] private ObservableCollection<Category> _categories = [];

    [ObservableProperty] private ObservableCollection<TransactionDto> _transactions = [];

    [ObservableProperty] private string _search = string.Empty;

    /// <inheritdoc/>
    public TransactionPageViewModel(IPersonRepository personRepository,
        ICategoryRepository categoryRepository,
        ITransactionRepository transactionRepository,
        IMessenger messenger) : base(messenger)
    {
        _personRepository = personRepository;
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
        _searchSubject = new Subject<string>();
        _searchDisposable = _searchSubject
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Subscribe((data) => _ = LoadTransactionsAsync());
    }

    public bool IsUpdateVisible => SelectedTransaction is not null;

    partial void OnSearchChanged(string value) => _searchSubject.OnNext(value);

    [RelayCommand]
    private void SelectTransaction(TransactionDto transaction)
    {
        SelectedTransaction = transaction;
    }

    [RelayCommand]
    private void UnselectTransaction()
    {
        SelectedTransaction = null;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        var loadPersonTask = LoadPersonAsync();
        var loadCategoryTask = LoadCategoryAsync();
        var loadTransactionTask = LoadTransactionsAsync();
        await Task.WhenAll(loadPersonTask, loadCategoryTask, loadTransactionTask);
    }

    [RelayCommand]
    private async Task UpdateTransactionAsync(Transaction transaction)
    {
        await _transactionRepository.UpdateAsync(transaction);
        UnselectTransaction();
    }

    [RelayCommand]
    private async Task DeleteTransactionAsync(Guid transactionId)
    {
        await _transactionRepository.DeleteAsync(transactionId);
    }

    [RelayCommand]
    private async Task CreateTransactionAsync(CreateTransactionDto dto)
    {
        await _transactionRepository.CreateAsync(dto.ToNewTransaction());
    }

    private async Task LoadTransactionsAsync()
    {
        IsLoading = true;
        var data = await _transactionRepository.GetAllAsync(Search);
        Transactions = new ObservableCollection<TransactionDto>(data);
        IsLoading = false;
    }

    private async Task LoadCategoryAsync()
    {
        var data = await _categoryRepository.GetAllAsync();
        Categories = new ObservableCollection<Category>(data);
    }


    private async Task LoadPersonAsync()
    {
        var data = await _personRepository.GetAllAsync();
        People = new ObservableCollection<Person>(data);
    }

    public void Receive(CsvImportedMessage message) => _ = LoadAsync();

    public void Receive(TransactionsChangedMessage message) => _ = LoadTransactionsAsync();

    public new void Dispose()
    {
        base.Dispose();
        _searchDisposable.Dispose();
    }
}