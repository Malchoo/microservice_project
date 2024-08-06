using ErrorOr;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class Ids
    {
        public static class BlockedId
        {
            public static readonly Error EmptyGuid = Error.Validation(
                code: "Ids.BlockedId.EmptyGuid",
                description: "Blocked Id cannot be an empty Guid.");
        }
    }
}