using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Dto;

public record ReportPayerDto(
    Person PersonToPay,
    decimal ValueToPay,
    List<TransactionShareDto> SharesToReceive,
    decimal SharesToReceiveTotal,
    List<TransactionShareDto> SharesToPay,
    decimal SharesToPayTotal
);