using ErrorOr;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class Ids
    {
        public static class FriendshipId
        {
            public static readonly Error EmptyGuid = Error.Validation(
                code: "Ids.FriendshipId.EmptyGuid",
                description: "Friendship Id cannot be an empty Guid.");
        }
    }
}