namespace Friendships.Write.Domain.ValueObjects;

public readonly record struct IsDeleted(bool Value)
{
    public static readonly IsDeleted No = new(false);
    public static readonly IsDeleted Yes = new(true);

    public string Name => Value ? "No" : "Yes";
}