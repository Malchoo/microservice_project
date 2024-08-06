using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Contracts;

namespace Friendships.Write.Domain.Ids;

public readonly record struct InvitationId : IEntityId
{
    public static InvitationId CreateUnique() => new(Guid.NewGuid());

    public InvitationId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentNullException(Errors.Ids.InvitationId.EmptyGuid.Description); //ToDo направи специална грешка

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