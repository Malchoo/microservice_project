using ErrorOr;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class Ids
    {
        public static class InvitedId
        {
            public static readonly Error EmptyGuid = Error.Validation(
                code: "Ids.InvitedId.EmptyGuid",
                description: "Invited Id cannot be an empty Guid.");
        }
    }
}