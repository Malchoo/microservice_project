using ErrorOr;
using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Enums;
using Friendships.Write.Domain.Ids;
using Friendships.Write.Domain.Primitives;
using Friendships.Write.Domain.Validators;
using System.Text.Json.Serialization;

namespace Friendships.Write.Domain.Entities;

public sealed class Friendship : Entity<FriendshipId>
{
    [JsonConstructor]
    private Friendship(
        FriendshipId id,
        UserId userId,
        FriendId friendId,
        InvitationId invitationId,
        FriendshipLevel type,
        FriendshipState status)
    {
        Id = id;
        UserId = userId;
        FriendId = friendId;
        InvitationId = invitationId;
        Level = type;
        State = status;
    }

    public UserId UserId { get; private set; }
    public FriendId FriendId { get; private set; }
    public InvitationId InvitationId { get; private set; }
    public FriendshipLevel Level { get; private set; } = null!;
    public FriendshipState State { get; private set; } = null!;

    internal static ErrorOr<Friendship> Create(
        UserId userId,
        FriendId friendId,
        InvitationId invitationId)
    {
        var friendshipId = FriendshipId.CreateUnique();

        var type = FriendshipLevel.Standard;

        var status = FriendshipState.Active;

        var friendship = new Friendship(friendshipId, userId, friendId, invitationId, type, status);

        var validationResult = new CreateFriendshipValidator().Validate(friendship);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
            .ToList();
            return errors;
        }
        return friendship;
    }

    internal static ErrorOr<Friendship> Accept(Friendship friendship)
    {
        var validationResult = new AcceptFriendshipValidator().Validate(friendship);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
            .ToList();
            return errors;
        }
        //ToDo прекалено много преобръщания има отдолу това трябва да се обмисли
        var temp = friendship.UserId.Value;
        friendship.UserId = new UserId(friendship.FriendId.Value);
        friendship.FriendId = new FriendId(temp);

        return friendship;
    }

    internal ErrorOr<Friendship> EndFriendship() => UpdateFriendshipState(FriendshipState.Ended);

    internal ErrorOr<Friendship> RestoreFriendship() => UpdateFriendshipState(FriendshipState.Active);

    internal ErrorOr<Friendship> ToTrustedUser() => UpdateFriendshipLevel(FriendshipLevel.Trusted);

    internal ErrorOr<Friendship> ToStandardFriend() => UpdateFriendshipLevel(FriendshipLevel.Standard);

    private ErrorOr<Friendship> UpdateFriendshipState(FriendshipState newStatus)
    {
        if (State == newStatus)
        {
            return newStatus == FriendshipState.Ended
                ? Errors.Entities.Friendship.AlreadyEnded
                : Errors.Entities.Friendship.NotEnded;
        }
        State = newStatus;
        return this;
    }

    private ErrorOr<Friendship> UpdateFriendshipLevel(FriendshipLevel newFriendshipLevel)
    {
        if (Level.Equals(newFriendshipLevel))
            return Errors.Entities.Friendship.SameStatus(newFriendshipLevel);

        Level = newFriendshipLevel;
        return this;
    }

    private Friendship()
    {
    }
}
