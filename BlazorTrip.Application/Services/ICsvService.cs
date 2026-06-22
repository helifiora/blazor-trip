namespace BlazorTrip.Application.Services;

public interface ICsvService
{
    Task ImportAsync(StreamReader reader);
    Task<byte[]> ExportAsync();
}