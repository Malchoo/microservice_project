using ErrorOr;

namespace Users.Domain.Errors.Settings;

public sealed class RolesErrors
{
    public static Error InvalidRole(string invalidRole) => Error.Validation(
        code: "RolesErrors.InvalidRole",
        description: $"Invalid role '{invalidRole}'.");
}
