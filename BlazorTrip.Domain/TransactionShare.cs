namespace BlazorTrip.Domain;

public record TransactionShare(
    Transaction Transaction,
    Guid ToPayId,
    Guid ToReceiveId,
    decimal ShareAmount
);