using BlazorTrip.Application.Dto;
using BlazorTrip.Application.Messages;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Domain.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Application.ViewModels;

public partial class ReportPageViewModel : BaseViewModel, IRecipient<CsvImportedMessage>
{
    private readonly IReportRepository _reportRepository;
    private readonly IPersonRepository _personRepository;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsPreviewVisible))]
    private TransactionDto? _previewTransaction;

    [ObservableProperty] private bool _isPersonLoading;

    [ObservableProperty] private bool _isReportLoading;

    [ObservableProperty] private Person? _selectedPerson;

    [ObservableProperty] private ReportDto? _report;

    [ObservableProperty] private List<Person> _people = [];

    public bool IsPreviewVisible => PreviewTransaction is not null;

    public ReportPageViewModel(IReportRepository reportRepository,
        IPersonRepository personRepository,
        IMessenger messenger) : base(messenger)
    {
        _reportRepository = reportRepository;
        _personRepository = personRepository;
    }

    partial void OnSelectedPersonChanged(Person? value)
    {
        if (value is null) return;
        _ = LoadPersonReportAsync();
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        await LoadPersonAsync();
        var first = People.FirstOrDefault();
        if (first is not null)
        {
            SelectedPerson = first;
        }
    }

    private async Task LoadPersonAsync()
    {
        IsPersonLoading = true;
        People = await _personRepository.GetAllAsync();
        IsPersonLoading = false;
    }

    private async Task LoadPersonReportAsync()
    {
        if (SelectedPerson is null) return;
        IsReportLoading = true;
        Report = await _reportRepository.GetPersonReportAsync(SelectedPerson.Id);
        IsReportLoading = false;
    }

    public void Receive(CsvImportedMessage message)
    {
        _ = LoadAsync();
        _ = LoadPersonReportAsync();
    }
}