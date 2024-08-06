using ErrorOr;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class Ids
    {
        public static class UserId
        {
            public static readonly Error EmptyGuid = Error.Validation(
                code: "Ids.UserId.EmptyGuid",
                description: "User Id cannot be an empty Guid.");
        }
    }
}