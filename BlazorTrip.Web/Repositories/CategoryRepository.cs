using BlazorTrip.Domain;
using BlazorTrip.Web.Components;

namespace BlazorTrip.Web.Repositories;

public class CategoryRepository : ICategoryRepository
{
    public event Action? OnChange;

    private readonly Dictionary<Guid, Category> _categories = [];

    public IEnumerable<Category> Categories => _categories.Select(s => s.Value);

    public CategoryRepository()
    {
        // var c = Category.Create("Carro", SelectCategoryIcon.AvailableIcons[1]);
        // _categories[c.Id] = c;
        // c = Category.Create("Moradia", SelectCategoryIcon.AvailableIcons[2]);
        // _categories[c.Id] = c;
        // c = Category.Create("Comida", SelectCategoryIcon.AvailableIcons[3]);
        // _categories[c.Id] = c;
        // c = Category.Create("Passeio", SelectCategoryIcon.AvailableIcons[4]);
        // _categories[c.Id] = c;
    }

    public Category? GetById(Guid id)
    {
        return _categories.GetValueOrDefault(id);
    }

    public Task<Category> Create(string name, string logo)
    {
        var category = Category.Create(name, logo);
        _categories[category.Id] = category;
        OnChange?.Invoke();
        return Task.FromResult(category);
    }

    public Task Delete(Guid id)
    {
        if (_categories.Remove(id))
        {
            OnChange?.Invoke();
        }

        return Task.CompletedTask;
    }
}