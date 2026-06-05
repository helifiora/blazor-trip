using BlazorTrip.Domain;
using BlazorTrip.Web.Data;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Web.Repositories;

public class DataCategoryRepository(DataState data) : ICategoryRepository
{
    public Task<List<Category>> GetMany()
    {
        return Task.FromResult(data.Categories.ToList());
    }

    public Task<Category?> GetById(Guid id)
    {
        return Task.FromResult(data.Categories.GetOrNull(id));
    }

    public Task Save(Category category)
    {
        try
        {
            var currentValue = data.Categories.GetOrNull(category.Id);
            if (currentValue is null)
            {
                data.Categories.Add(category);
                WeakReferenceMessenger.Default.Send(new CategoryAddedMessage(category));
            }
            else
            {
                data.Categories.Update(category);
                WeakReferenceMessenger.Default.Send(new CategoryUpdatedMessage(category));
            }

            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }
    }

    public Task Delete(Guid id)
    {
        try
        {
            if (data.Categories.Remove(id))
            {
                WeakReferenceMessenger.Default.Send(new CategoryDeletedMessage(id));
            }
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.CompletedTask;
        }
    }

    public Task Import(IEnumerable<Category> categories)
    {
        var categoryData = categories.ToList();
        data.Categories.Set(categoryData);
        WeakReferenceMessenger.Default.Send(new CategoryImportedMessage(categoryData));
        return Task.CompletedTask;
    }
}