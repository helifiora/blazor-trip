namespace BlazorTrip.Domain.Models;

public record Person(Guid Id, string Name) : IEntity
{
    public static Person Create(string name)
    {
        return new Person(Guid.NewGuid(), name);
    }
}