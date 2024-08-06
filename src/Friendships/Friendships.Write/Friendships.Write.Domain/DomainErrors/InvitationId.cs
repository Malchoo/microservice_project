using ErrorOr;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class Ids
    {
        public static class InvitationId
        {
            public static readonly Error EmptyGuid = Error.Validation(
                code: "Ids.InvitationId.EmptyGuid",
                description: "Invitation Id cannot be an empty Guid.");
        }
    }
}