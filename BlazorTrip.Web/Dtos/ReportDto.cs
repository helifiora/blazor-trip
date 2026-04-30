using BlazorTrip.Domain;

namespace BlazorTrip.Web.Dtos;

public record ReportDto(
    Person Person,
    decimal TotalSpent,
    decimal Balance,
    IEnumerable<ReportPayerDto> TransactionShares
);

public record ReportPayerDto(
    Person PersonToPay,
    decimal ValueToPay,
    IEnumerable<ReportShareDto> Shares
);

public record ReportShareDto(
    Transaction Transaction,
    Person PersonToPay,
    Person PersonToReceive,
    decimal SharedAmount,
    Category Category
);