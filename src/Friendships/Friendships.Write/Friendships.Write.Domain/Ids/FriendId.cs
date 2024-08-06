using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Contracts;

namespace Friendships.Write.Domain.Ids;

public readonly record struct FriendId : IEntityId, IEquatable<FriendId>
{
    public static FriendId CreateUnique() => new(Guid.NewGuid());

    public FriendId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException(Errors.Ids.FriendId.EmptyGuid.Description); //ToDo направи специална грешка

        Value = value;
    }

    public Guid Value { get; init; }

    public bool Equals(FriendId other)
    {
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}