using BlazorTrip.Application.Messages;
using BlazorTrip.Application.Repositories;
using BlazorTrip.Domain.Models;
using BlazorTrip.Infrastructure.InMemory;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Infrastructure.Repositories;

public class MemoryCategoryRepository(InMemoryDaoContext dao, IMessenger messenger) : ICategoryRepository
{
    public Task<List<Category>> GetAllAsync()
    {
        var result = dao.Categories.Items
            .OrderBy(s => s.Name)
            .ToList();

        return Task.FromResult(result);
    }

    public Task CreateAsync(Category category)
    {
        if (dao.Categories.Add(category)) NotifyChange();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid categoryId)
    {
        if (HasTransactionsWith(categoryId)) return Task.FromException(new Exception());
        if (dao.Categories.Remove(categoryId)) NotifyChange();
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Category category)
    {
        if (dao.Categories.Update(category)) NotifyChange();
        return Task.CompletedTask;
    }

    private bool HasTransactionsWith(Guid categoryId)
    {
        return dao.Transactions.GetWhere(s => s.CategoryId == categoryId) != null;
    }

    private void NotifyChange() => messenger.Send(new CategoriesChangedMessage());
}