using ErrorOr;

namespace Users.Domain.Errors.Settings;

public static class IsEmailVerifiedErrors
{
    public static Error CannotBeNull => Error.Validation(
        code: "IsEmailVerifiedErrors.CannotBeNull",
        description: "Email cannot be null");
}
