using BlazorTrip.Domain;
using BlazorTrip.Web.Repositories;

namespace BlazorTrip.Web.Commands;

public class CreateTransactionCommand(
    IPersonRepository personRepo,
    ICategoryRepository categoryRepo,
    ITransactionRepository transactionRepo
)
{
    public Task Execute(Transaction transaction)
    {
        if (categoryRepo.GetById(transaction.CategoryId) is null)
            throw new Exception("Category not found!");

        if (personRepo.GetById(transaction.PayerId) is null)
            throw new Exception("Payer not found!");

        if (transaction.SharedIds.Any(id => personRepo.GetById(id) is null))
            throw new Exception("Shared person {id} not found!");

        transactionRepo.Create(transaction);
        return Task.CompletedTask;
    }
}