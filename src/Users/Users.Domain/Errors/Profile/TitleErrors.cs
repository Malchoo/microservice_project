using ErrorOr;
using Users.Domain.Contracts;

namespace Users.Domain.Errors.Profile;

public static class TitleErrors
{
    public static readonly Error CannotConvertFromName = Error.Validation(

        code: "TitleErrors.CannotConvertFromValue",
        description: $"Cannot convert to a valid title.");

    public static Error CannotChangeStatus(string currentStatus, string attemptedStatus)
        => Error.Conflict(
            code: "TitleErrors.CannotChangeStatus",
            description: $"Cannot change title from {currentStatus} to {attemptedStatus}.");
}

