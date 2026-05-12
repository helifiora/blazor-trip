using BlazorTrip.Domain;
using BlazorTrip.Web.Data;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Web.Repositories;

public class DataCategoryRepository(DataState data) : ICategoryRepository
{
    public event Action? OnChange;

    public IEnumerable<Category> Categories => data.Categories;

    public Category? GetById(Guid id) => data.Categories.GetOrNull(id);

    public Task Save(string name, string logo)
    {
        try
        {
            var category = Category.Create(name, logo);
            data.Categories.Add(category);
            NotifyChangedState();
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }
    }

    public Task Update(Category category)
    {
        try
        {
            var currentValue = data.Categories.GetOrNull(category.Id);
            if (currentValue is null)
                return Task.CompletedTask;

            data.Categories.Update(category);
            NotifyChangedState();
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
                NotifyChangedState();
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
        data.Categories.Set(categories);
        NotifyChangedState();
        return Task.CompletedTask;
    }

    public void NotifyChangedState()
    {
        OnChange?.Invoke();
        WeakReferenceMessenger.Default.Send(new CategoryChangedMessage(Categories.ToList()));
    }
}