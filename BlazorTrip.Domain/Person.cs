namespace BlazorTrip.Domain;

public record Person(Guid Id, string Name)
{
    public static Person Create(string name)
    {
        return new Person(Guid.NewGuid(), name);
    }
}