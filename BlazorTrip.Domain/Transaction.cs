namespace BlazorTrip.Domain;

public record Transaction(
    Guid Id,
    string Name,
    decimal Amount,
    Guid CategoryId,
    Guid PayerId,
    IEnumerable<Guid> SharedIds
)
{
    public static Transaction Create(
        string name,
        decimal amount,
        Guid categoryId,
        Guid payerId,
        IEnumerable<Guid> sharedIds
    )
    {
        return new Transaction(Guid.NewGuid(), name, amount, categoryId, payerId, sharedIds);
    }

    public decimal CalculateAmountPerPerson()
    {
        if (!SharedIds.Any()) return 0;
        return Amount / SharedIds.Count();
    }

    public IEnumerable<TransactionShare> CalculateTransactionShares()
    {
        var amountPerPerson = CalculateAmountPerPerson();
        return SharedIds
            .Where(s => s != PayerId)
            .Select(s => new TransactionShare(this, s, PayerId, amountPerPerson));
    }
}