namespace BlazorTrip.Domain.Models;

public class CsvData
{
    public required string Name { get; init; }

    public required decimal Value { get; init; }

    public required string Payer { get; init; }

    public required string SharedBy { get; init; }

    public required string Category { get; init; }

    public required string CategoryLogo { get; init; }

    public List<string> SharedByList => SharedBy
        .Split(',')
        .Where(q => !string.IsNullOrWhiteSpace(q))
        .Select(s => s.Trim())
        .ToList();
}