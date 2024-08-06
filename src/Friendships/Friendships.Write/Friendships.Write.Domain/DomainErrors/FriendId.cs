using ErrorOr;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class Ids
    {
        public static class FriendId
        {
            public static readonly Error EmptyGuid = Error.Validation(
                code: "Ids.FriendId.EmptyGuid",
                description: "Friend Id cannot be an empty Guid.");
        }
    }
}