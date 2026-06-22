using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Dto;

public record TransactionDto(
    Guid Id,
    string Name,
    decimal Amount,
    Category Category,
    Person Payer,
    IEnumerable<Person> Shared
);

public record TransactionShareDto(
    TransactionDto Transaction,
    Person ToPay,
    Person ToReceive,
    decimal ShareAmount
);