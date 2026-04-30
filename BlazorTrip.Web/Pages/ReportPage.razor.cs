using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Facade;
using BlazorTrip.Web.Repositories;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Pages;

public partial class ReportPage(
    ReportFacade reportFacade,
    IPersonRepository personRepository
) : ComponentBase
{
    private bool _previewIsVisible;

    private List<ReportDto> _reports = [];

    protected override void OnInitialized()
    {
        _reports = personRepository.People
            .Select(p => reportFacade.GenerateReportFrom(p.Id))
            .ToList();
    }
}