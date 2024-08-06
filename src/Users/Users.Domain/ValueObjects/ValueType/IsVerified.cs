namespace Users.Domain.ValueObjects.ValueType;

public readonly record struct IsVerified(bool Value)
{
    public static readonly IsVerified Yes = new(true);
    public static readonly IsVerified No = new(false);

    public string Name => Value ? "Yes" : "No";
}