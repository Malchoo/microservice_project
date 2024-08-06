using ErrorOr;

namespace Users.Domain.Errors.Settings;

public static class IsMobileNumberVerifiedErrors
{
    public static Error CannotBeNull => Error.Validation(
        code: "IsMobileNumberVerified.CannotBeNull",
        description: "Mobile number verification status cannot be null.");
}