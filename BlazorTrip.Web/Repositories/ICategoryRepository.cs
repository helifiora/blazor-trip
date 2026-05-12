using BlazorTrip.Domain;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BlazorTrip.Web.Repositories;

public interface ICategoryRepository
{
    event Action? OnChange;
    IEnumerable<Category> Categories { get; }
    Category? GetById(Guid id);
    Task Save(string name, string logo);
    Task Update(Category category);
    Task Delete(Guid id);
    Task Import(IEnumerable<Category> categories);
    void NotifyChangedState();
}

public class CategoryChangedMessage(List<Category> categories) : 
    ValueChangedMessage<List<Category>>(categories);