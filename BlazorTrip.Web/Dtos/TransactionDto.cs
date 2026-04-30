using BlazorTrip.Domain;

namespace BlazorTrip.Web.Dtos;

public record TransactionDto(
    Guid Id,
    string Name,
    decimal Amount,
    Category Category,
    Person Payer,
    IEnumerable<Person> Shared
);