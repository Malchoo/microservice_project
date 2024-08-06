namespace Users.Domain.ValueObjects.ValueType;

public readonly record struct IsDeleted(bool Value)
{
    public static readonly IsDeleted Yes = new(true);
    public static readonly IsDeleted No = new (false);

    public string Name => Value ? "Yes" : "No";
}