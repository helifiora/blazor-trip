namespace BlazorTrip.Domain;

public record Category(
    Guid Id,
    string Name,
    string Logo
)
{
    public static Category Create(string name, string logo)
    {
        return new Category(Guid.NewGuid(), name, logo);
    }
}