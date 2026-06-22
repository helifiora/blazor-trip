using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Dto;

public record ReportDto(
    Person Person,
    decimal TotalSpent,
    decimal Balance,
    List<ReportPayerDto> ReportPayers,
    List<TransactionDto> Transactions
);