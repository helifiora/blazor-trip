using BlazorTrip.Domain;
using BlazorTrip.Web.Dtos;
using BlazorTrip.Web.Repositories;

namespace BlazorTrip.Web.Queries;

public class SelectTransactionDto(
    IPersonRepository personRepo,
    ICategoryRepository categoryRepo,
    ITransactionRepository transactionRepo
)
{
    public IQueryable<TransactionDto> GetAll()
    {
        return transactionRepo.Transactions
            .AsEnumerable()
            .Select(ConvertToDto)
            .AsQueryable();
    }

    private TransactionDto ConvertToDto(Transaction transaction)
    {
        var category = categoryRepo.GetById(transaction.CategoryId);
        if (category is null)
            throw new Exception("Category not found");
        var payer = personRepo.GetById(transaction.PayerId);
        if (payer is null)
            throw new Exception("Payer not found");

        var rawShared = transaction.SharedIds.Select(personRepo.GetById);
        if (rawShared.Any(l => l is null))
            throw new Exception("Shared is null");

        var shared = rawShared.OfType<Person>();
        return new TransactionDto(
            transaction.Id,
            transaction.Name,
            transaction.Amount,
            category,
            payer,
            shared
        );
    }
}