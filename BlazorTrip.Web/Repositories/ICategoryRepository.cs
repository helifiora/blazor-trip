using BlazorTrip.Domain;
using BlazorTrip.Domain.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BlazorTrip.Web.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetMany();
    Task<Category?> GetById(Guid id);
    Task Save(Category category);
    Task Delete(Guid id);
    Task Import(IEnumerable<Category> categories);
}
    
public class CategoryImportedMessage(List<Category> people) : ValueChangedMessage<List<Category>>(people);

public class CategoryAddedMessage(Category person) : ValueChangedMessage<Category>(person);

public class CategoryUpdatedMessage(Category person) : ValueChangedMessage<Category>(person);

public class CategoryDeletedMessage(Guid id) : ValueChangedMessage<Guid>(id);