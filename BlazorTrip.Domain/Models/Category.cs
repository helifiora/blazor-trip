namespace BlazorTrip.Domain.Models;

public record Category(Guid Id, string Name, string Logo) : IEntity
{
    public static Category Create(string name, string logo)
    {
        return new Category(Guid.NewGuid(), name, logo);
    }
}

