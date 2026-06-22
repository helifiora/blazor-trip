namespace BlazorTrip.Application.Repositories;

public interface IDataRepository
{
    Task<byte[]> ExportAsync();
    Task ImportAsync(StreamReader stream);
}