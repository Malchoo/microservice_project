using ErrorOr;
using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Entities;
using Friendships.Write.Domain.Enums;
using Friendships.Write.Domain.Ids;
using Friendships.Write.Domain.Primitives;
using System.Collections.Frozen;

namespace Friendships.Write.Domain.ValueObjects;

public sealed class FriendshipCollection : ValueObject
{
    private Dictionary<FriendId, Friendship> _friendships = new();
    private readonly FriendshipState _status;

    private FriendshipCollection(FriendshipState status) => _status = status;

    public int Count => _friendships.Count;

    public FrozenDictionary<FriendId, Friendship> Friendships => _friendships.ToFrozenDictionary();

    public static FriendshipCollection Empty(FriendshipState status) => new(status);

    public ErrorOr<Success> Add(Friendship friendship)
    {
        if (friendship == null)
            return Errors.ValueObjects.FriendshipCollection.NullFriendship;

        if (friendship.State != _status)
            return Errors.ValueObjects.FriendshipCollection.StatusMismatch(_status, friendship.State);

        if (_friendships.ContainsKey(friendship.FriendId))
            return Errors.ValueObjects.FriendshipCollection.FriendshipAlreadyExists;

        _friendships.Add(friendship.FriendId, friendship);

        return new Success();
    }

    public ErrorOr<Success> Remove(Friendship friendship)
    {
        if (friendship.State != _status)
            return Errors.ValueObjects.FriendshipCollection.StatusMismatch(_status, friendship.State);

        if (!_friendships.ContainsKey(friendship.FriendId))
            return Errors.ValueObjects.FriendshipCollection.FriendshipNotFound;

        _friendships.Remove(friendship.FriendId);
        
        return new Success();
    }

    internal bool Contains(IEntityId id) => Contains(id.Value);

    internal bool Contains(Friendship friendship) => Contains(friendship.FriendId.Value);

    internal bool ContainsValue(Friendship friendship) => _friendships.ContainsValue(friendship);

    internal ErrorOr<Friendship> GetFriendshipByFriendshipId(IEntityId id)
    {
        if (_friendships.TryGetValue(new FriendId(id.Value), out var friendship))
            return friendship;

        return Errors.Entities.FriendshipList.NotFound(id);
    }

    private bool Contains(Guid id) => _friendships.ContainsKey(new FriendId(id));

    private bool IsEmptyGuid(FriendId friendshipId) => friendshipId.Value == Guid.Empty;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return _friendships;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not FriendshipCollection other)
            return false;

        if (_friendships.Count != other._friendships.Count)
            return false;

        foreach (var friendship in _friendships)
        {
            if (!other._friendships.TryGetValue(friendship.Key, out var otherFriendship))
                return false;

            if (!friendship.Value.Equals(otherFriendship))
                return false;
        }

        return true;
    }

    public override int GetHashCode() => _friendships.Aggregate(0, (hash, friendship)
        => HashCode.Combine(hash, friendship.Key, friendship.Value));

    private FriendshipCollection()
    {
    }
}
