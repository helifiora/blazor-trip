using System.ComponentModel;
using BlazorTrip.Domain;

namespace BlazorTrip.Web.Repositories;

public interface ICategoryRepository
{
    public event Action? OnChange;
    public IEnumerable<Category> Categories { get; }
    public Category? GetById(Guid id);
    public Task<Category> Create(string name, string logo);
    public Task Delete(Guid id);
    
    public Task Import(IEnumerable<Category> categories);
}