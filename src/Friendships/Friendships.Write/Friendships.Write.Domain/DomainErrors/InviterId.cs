using ErrorOr;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class Ids
    {
        public static class InviterId
        {
            public static readonly Error EmptyGuid = Error.Validation(
                code: "Ids.InviterId.EmptyGuid",
                description: "Inviter Id cannot be an empty Guid.");
        }
    }
}