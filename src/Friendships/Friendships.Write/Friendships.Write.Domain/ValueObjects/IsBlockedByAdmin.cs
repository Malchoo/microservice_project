namespace Friendships.Write.Domain.ValueObjects;

public readonly record struct IsBlockedByAdmin(bool Value)
{
    public static readonly IsBlockedByAdmin Yes = new(true);
    public static readonly IsBlockedByAdmin No = new(false);

    public string Name => Value ? "Yes" : "No";

}