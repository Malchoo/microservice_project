using ErrorOr;

namespace Friendships.Domain.DomainErrors;

public static partial class Errors
{
    public static partial class ValueObjects
    {
        public static class FriendshipLevel
        {
            public static Error CannotChangeStatus(
                string currentStatus, string attemptedStatus) => Error.Conflict(
                code: "ValueObjects.FriendshipLevel.CannotChangeStatus",
                description: $"Cannot change relationship type from {currentStatus} to {attemptedStatus}.");
        }
    }
}