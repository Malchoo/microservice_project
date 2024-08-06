using ErrorOr;

namespace Friendships.Domain.DomainErrors;
public static partial class Errors
{
    public static partial class ValueObjects
    {
        public static class TrustedUser
        {
            public static readonly Error NotTrustedUserInvitation = Error.Validation(
                "ValueObjects.TrustedUser.NotTrustedUserInvitation",
                "Invitation is not a trusted user invitation.");

            public static readonly Error NotPendingInvitation = Error.Validation(
                "ValueObjects.TrustedUser.NotPendingInvitation",
                "Invitation status is not pending.");
        }
    }
}