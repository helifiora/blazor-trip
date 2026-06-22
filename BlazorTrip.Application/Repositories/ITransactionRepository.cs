using BlazorTrip.Application.Dto;
using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Repositories;

public interface ITransactionRepository
{
    Task<List<TransactionDto>> GetAllAsync(string search = "");
    Task CreateAsync(Transaction transaction);
    Task DeleteAsync(Guid transactionId);
    Task UpdateAsync(Transaction transaction);
}