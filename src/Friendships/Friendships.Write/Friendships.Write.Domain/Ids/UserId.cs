using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Contracts;

namespace Friendships.Write.Domain.Ids;

public readonly record struct UserId : IEntityId
{
    public static UserId CreateUnique() => new(Guid.NewGuid());

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException(Errors.Ids.UserId.EmptyGuid.Description); //ToDo направи специална грешка

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