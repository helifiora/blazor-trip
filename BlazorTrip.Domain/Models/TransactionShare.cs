namespace BlazorTrip.Domain.Models;

public record TransactionShare(
    Transaction Transaction,
    Guid ToPayId,
    Guid ToReceiveId,
    decimal ShareAmount
);