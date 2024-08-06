using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Contracts;

namespace Friendships.Write.Domain.Ids;

public readonly record struct BlockedId : IEntityId
{
    public static BlockedId CreateUnique() => new(Guid.NewGuid());

    public BlockedId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException(Errors.Ids.BlockedId.EmptyGuid.Description); //ToDo направи специална грешка

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