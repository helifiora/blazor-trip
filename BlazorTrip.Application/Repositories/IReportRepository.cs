using BlazorTrip.Application.Dto;

namespace BlazorTrip.Application.Repositories;

public interface IReportRepository
{
    Task<ReportDto> GetPersonReportAsync(Guid personId);
}