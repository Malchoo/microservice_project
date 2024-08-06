using ErrorOr;
using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Enums;
using Friendships.Write.Domain.Ids;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class ValueObjects
    {
        public static class FriendshipCollection
        {
            public static readonly Error NotEmptyGuid = Error.Validation(
                code: "ValueObjects.FriendshipCollection.NotGuid",
                description: "Friendship Id cannot be an empty Guid.");

            public static readonly Error AlreadyInFriendshipCollection = Error.Conflict(
                code: "ValueObjects.FriendshipCollection.AlreadyInFriendshipCollection",
                description: "Friendship is already in the FriendshipCollection. You cannot add it again.");

            public static Error WrongUser(UserId userId) => Error.Validation(
                code: "ValueObjects.FriendshipCollection.WrongUser",
                description: $"Friendship UserId '{userId}' does not match the UserId of the FriendshipCollection.");

            public static Error WrongStatus(string status) => Error.Validation(
                code: "ValueObjects.FriendshipCollection.WrongStatus",
                description: $"Friendship Status '{status}' does not match the Status of the FriendshipCollection.");

            public static readonly Error Empty = Error.Conflict(
                code: "ValueObjects.FriendshipCollection.Empty",
                description: "FriendshipCollection is empty.");

            public static Error CannotRemoveFriendship(IEntityId id) => Error.NotFound(
                code: "ValueObjects.FriendshipCollection.CannotRemoveFriendship",
                description: $"Friendship with FriendId '{id}' not found in the FriendshipCollection.");
            public static readonly Error FriendshipNotFound = Error.NotFound(
                    code: "ValueObjects.FriendshipCollection.FriendshipNotFound",
                    description: "Friendship not found in the dictionary.");

            public static readonly Error FriendshipAlreadyExists = Error.Conflict(
                code: "ValueObjects.FriendshipCollection.FriendshipAlreadyExists",
                description: "Friendship already exists in the dictionary.");

            public static readonly Error Invalid
                = Error.Validation(
                code: "ValueObjects.FriendshipCollection.InvalidFriendshipState",
                description: "Invalid friendship status.");

            public static readonly Error NullFriendship = Error.Validation(
                code: "ValueObjects.FriendshipCollection.NullFriendship",
                description: "Cannot add null friendship to the dictionary."); 
            
            public static Error StatusMismatch(FriendshipState expected, FriendshipState actual) => Error.Validation(
                code: "ValueObjects.FriendshipCollection.StatusMismatch",
                description: $"Cannot add a friendship with status {actual} to a dictionary for {expected} friendships.");
        }
    }
}