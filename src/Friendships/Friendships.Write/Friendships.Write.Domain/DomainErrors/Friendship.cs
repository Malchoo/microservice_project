using ErrorOr;
using Friendships.Write.Domain.Enums;
using Friendships.Write.Domain.Ids;

namespace Friendships.Domain.DomainErrors;

public static partial class Errors
{
    public static partial class Entities
    {
        public static class Friendship
        {
            public static readonly Error InvalidId = Error.Validation(
                code: "Entities.Friendship.InvalidId",
                description: "Friendship Id must be a valid non-empty GUID.");

            public static readonly Error InvalidUserId = Error.Validation(
                code: "Entities.Friendship.InvalidUserId",
                description: "User Id must be a valid non-empty GUID.");

            public static readonly Error InvalidFriendId = Error.Validation(
                code: "Entities.Friendship.InvalidFriendId",
                description: "Friend Id must be a valid non-empty GUID.");

            public static readonly Error InvalidInvitationId = Error.Validation(
                code: "Entities.Friendship.InvalidInvitationId",
                description: "Invitation Id must be a valid non-empty GUID.");

            public static readonly Error SelfFriendship = Error.Validation(
                code: "Entities.Friendship.SelfFriendship",
                description: "You can't add yourself as a friend");

            public static Error InvitationIdDoseNotMatch(InvitationId invitationId) => Error.NotFound(
                code: "Entities.Friendship.InvitationIdDoseNotMatch",
                description: $"Invitation Id '{invitationId}' does not match.");

            public static readonly Error LevelCannotBeEmpty = Error.Validation(
                code: "Entities.Friendship.LevelCannotBeEmpty",
                description: "Friendship type must be provided and cannot be empty.");

            public static readonly Error InvalidCreationLevel = Error.Validation(
                code: "Entities.Friendship.InvalidCreationLevel",
                description: "Friendship type shoud be 'standard' when creating friendship.");

            public static readonly Error StatusCannotBeEmpty = Error.Validation(
                code: "Entities.Friendship.StatusCannotBeEmpty",
                description: "Friendship status must be provided and cannot be empty.");

            public static readonly Error InvalidStatus = Error.Validation(
                code: "Entities.Friendship.InvalidStatus",
                description: "Friendship status shoud be 'active' when creating or accepting friendship.");

            public static readonly Error AlreadyEnded = Error.Conflict(
                code: "Entities.Friendship.AlreadyEnded",
                description: "Friendship is already ended. You cannot end it one more time.");

            public static readonly Error NotEnded = Error.Conflict(
                code: "Entities.Friendship.NotEnded",
                description: "Friendship is not ended.You can restore only ended friendships.");

            public static Error SameStatus(FriendshipLevel friendshipLevel) => Error.NotFound(
                code: "Entities.Friendship.SameStatus",
                description: $"You cannot change the status {friendshipLevel.Name}. " +
                $"The friendship is already {friendshipLevel.Name}.");
        }
    }
}