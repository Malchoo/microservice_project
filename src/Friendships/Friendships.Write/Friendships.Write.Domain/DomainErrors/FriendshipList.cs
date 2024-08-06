using ErrorOr;
using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Enums;
using Friendships.Write.Domain.Ids;

namespace Friendships.Domain.DomainErrors;

public static partial class Errors
{
    public static partial class Entities
    {
        public static class FriendshipList

        {
            public static readonly Error InvalidUserId = Error.Validation(
            code: "Entities.FriendshipList.InvalidUserId",
            description: "User Id must be a valid non-empty GUID.");

            public static Error WrongRecipient(IEntityId id) => Error.Validation(
                code: "Entities.FriendshipList.WrongRecipient",
                description: $"The user ID '{id.Value}' is not the recipient of this friendship.");

            public static Error CannotRejectWrongRecipient(FriendshipId friendshipId, UserId userId) => Error.Validation(
                code: "Entities.FriendshipList.CannotRejectWrongRecipient",
                description: $"You cannot reject the friendship with friendship ID '{friendshipId.Value}'. " +
                $"The user ID '{userId.Value}' is not the recipient of this friendship.");

            public static Error FriendIdDoseNotMatch(FriendId friendId) => Error.Conflict(
                code: "Entities.FriendshipList.FriendIdDoseNotMatch",
                description: $"The friend ID '{friendId.Value}' does not match the friend ID of this friendship.");

            public static Error AlreadyFriend(FriendId friendId) => Error.Conflict(
                code: "Entities.FriendshipList.AlreadyFriend",
                description: $"The user with ID '{friendId.Value}' is already your friend.");

            public static Error AlreadyFriend(InviterId inviterId) => Error.Conflict(
                code: "Entities.FriendshipList.AlreadyFriend",
                description: $"You cannot create friendship with the user with ID '{inviterId.Value}' " +
                $"The inviter with Id '{inviterId.Value}' is now or was before, your friend. ");

            public static Error CannotAcceptAlreadyFriend(FriendId friendId) => Error.Conflict(
                code: "Entities.FriendshipList.CannotAcceptAlreadyFriend",
                description: $"You cannot accept the friendship with user with ID '{friendId.Value}' " +
                $"The user with ID '{friendId.Value}' is already your friend.");

            public static readonly Error CannotBeDeletedWhenCreating = Error.Validation(
                code: "Entities.FriendshipList.CannotBeDeletedWhenCreating",
                description: "You cannot create friendship list and the list to be deleted at the same time. " +
                "You can delete the user after the friendship is created.");

            public static readonly Error CannotBeBlockedByAdminWhenCreating = Error.Validation(
                code: "Entities.FriendshipList.CannotBeBlockedByAdminWhenCreating",
                description: "You cannot create friendship list and the list to be blocked by admin at the same time. " +
                "You can block the user by admin after the friendship is created.");

            public static Error AlreadyBlockedUser(BlockedId blockedId) => Error.Conflict(
                code: "Entities.FriendshipList.AlreadyBlockedUser",
                description: $"User with Id '{blockedId.ToString()}' is already blocked.");

            public static Error UserBlocked(InviterId inviterId) => Error.Conflict(
                code: "Entities.FriendshipList.UserBlocked",
                description: $"You cannot create friendship with the user with ID '{inviterId.Value}' " +
                $"The user with ID '{inviterId.Value}' is blocked " +
                $"or you are blocked by user eith ID '{inviterId.Value}'.");

            public static Error InvitationAlreadyUsed(InvitationId invitationId) => Error.Conflict(
                code: "Entities.FriendshipList.InvitationAlreadyUsed",
                description: $"The invitation with ID '{invitationId.Value}' is already used. " +
                "You cannot use it again.");

            public static Error CannotAccpetInvitationIdAlreadyUsed(InvitationId invitationId) => Error.Conflict(
                code: "Entities.FriendshipList.CannotAccpetInvitationAlreadyUsed",
                description: $"You cannot accept the friendship with invitation ID '{invitationId.Value}'. " +
                $"The invitation with ID '{invitationId.Value}' is already used.");

            public static readonly Error UserNotBlocked = Error.NotFound(
                code: "Entities.FriendshipList.UserNotBlocked",
                description: "The user is not blocked. You cannot unblock the user.");

            public static Error InvitationNotAvailable(InvitationId invitationId) => Error.Conflict(
                code: "Entities.FriendshipList.InvitationNotAvailable",
                description: $"The invitation with ID '{invitationId.Value}' is not available. " +
                "You cannot use it. " +
                $"You cannot end friendship with invitation ID '{invitationId.Value}'");
            public static Error AlreadyEndedFriendship(FriendshipLevel friendshipLevel) => Error.Conflict(
                code: "Entities.FriendshipList.AlreadyEndedFriendship",
                description: $"This friendship is ended. You cannot change relation status to {friendshipLevel}. " +
                "You shoud restore the friendship first, then you can try to change " +
                $"the relation status to {friendshipLevel}.");

            public static Error AlreadyEndedFriendship(FriendId friendId) => Error.Conflict(
                code: "Entities.FriendshipList.AlreadyEndedFriendship",
                description: $"This friendship with user with user ID '{friendId}'is ended. " +
                $"You shoud recover the friendship first. ");

            public static readonly Error UserBlockedByAdmin = Error.Conflict(
                code: "Entities.FriendshipList.UserBlockedByAdmin",
                description: "The user is blocked by admin. You cannot create friendship with this user.");

            public static Error AlreadyEndedFriendship() => Error.Conflict(
                code: "Entities.FriendshipList.AlreadyEndedFriendship",
                description: "This friendship is ended. " +
                "You cannot end it one more time. " +
                "You cannot create it again, you shoud restore it." +
                "You cannot change status. " +
                "The user was your friend before. If you wish so, ");

            public static readonly Error FriendsIsDeleted = Error.Conflict(
                code: "Entities.FriendshipList.FriendsIsDeleted",
                description: "The friendship is deleted. You should restore it first.");

            public static Error AlreadyEndedFriendship(InviterId inviterId) => Error.Conflict(
                code: "Entities.FriendshipList.AlreadyEndedFriendship",
                description: $"This friendship with user with user ID '{inviterId}'is ended. You cannot create it new one. " +
                $"You shoud restore t.");

            public static Error InvitationIdMissMatch(InvitationId invitationId) => Error.Conflict(
                code: "Entities.FriendshipList.InvitationIdMissMatch",
                description: $"The invitation ID '{invitationId.Value}' does not match with the invitation ID of this friendship.");

            public static Error ReachedMaxFriendshipsCount(int maxFriendsCount) => Error.Validation(
                code: "Entities.FriendshipList.ReachedMaxFriendshipsCount",
                description: $"You have reached the maximum number of friendships of '{maxFriendsCount}'.");

            public static readonly Error MissingFriendship = Error.Conflict(
                code: "Entities.FriendshipList.MissingFriendship",
                description: "Friendship is missing. You cannot do anything to a " +
                "friendship that does not exist.");

            public static Error NotFound(IEntityId userId) => Error.NotFound(
                code: "Entities.FriendshipList.NotFound",
                description: $"User not found. This user with Id '{userId.Value.ToString()}' " +
                $"is not be part of this database.");

            public static readonly Error BlockedByAdmin = Error.Conflict(
                code: "Entities.FriendshipList.BlockedByAdmin",
                description: "The user is blocked by admin.");

            public static Error NotFound(InvitationId invitationId) => Error.NotFound(
                code: "Entities.FriendshipList.NotFound",
                description: $"Friendship with invitation ID '{invitationId.Value}' not found. " +
                "This friendship may not be part of this FriendshipAggregate.");

            public static readonly Error SlefFriend = Error.Validation(
                code: "Entities.FriendshipList.SlefFriend",
                description: "You cannot create friendship with yourself. " +
                "You cannot be your own friend.");

            public static Error InvitationIdDoseNotMatch(InvitationId invitationId) => Error.NotFound(
                code: "Entities.FriendshipList.InvitationIdDoseNotMatch",
                description: $"Invitation ID '{invitationId.Value}' does not match with friendship invitation Id.");

            public static Error CannotRejectInvitationIdMissMatch(InvitationId invitationId) => Error.NotFound(
                code: "Entities.FriendshipList.CannotRejectInvitationIdMissMatch",
                description: $"Friendship with invitation ID '{invitationId.Value}' was rejected, but the invitation Id is " +
                $"not part of your list with invitation Ids.");
        }
    }
}