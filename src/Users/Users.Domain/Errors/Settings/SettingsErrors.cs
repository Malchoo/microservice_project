using ErrorOr;
using Users.Domain.ValueObjects.ValueType;

namespace Users.Domain.Errors.Settings;

public sealed class SettingsErrors
{
    public static readonly Error NoRolesToAdd = Error.Validation(
        code: "SettingsErrors.NoRolesToAdd",
        description: "No roles to add.");

    public static readonly Error RoleNotFound = Error.NotFound(
        code: "SettingsErrors.RoleNotFound",
        description: "Role not found.");

    public static readonly Error CannotRemoveRoleUser = Error.Validation(
        code: "SettingsErrors.CannotRemoveRoleUser",
        description: "Cannot remove user role.");

    public static readonly Error BlockedByAdmin = Error.Conflict(
        code: "SettingsErrors.BlockedByAdmin",
        description: "User is blocked by admin.");

    public static Error AlreadyBlockedByAdmin(IsBlockedByAdmin isBlockedByAdmin) => Error.Conflict(
        code: "SettingsErrors.AlreadyBlockedByAdmin",
        description: "User is already blocked by admin. " +
        $"Now you try to set it to '{isBlockedByAdmin.Name}'.");

    public static Error IsVerifiedNotChanged(IsVerified isVerified) => Error.Conflict(
        code: "SettingsErrors.IsVerifiedNotChanged",
        description: "IsVerified is not changed. " +
        $"Now you try to set it to '{isVerified.Name}'.");

    public static Error NotBlockedByAdmin(IsBlockedByAdmin isBlockedByAdmin) => Error.Conflict(
        code: "SettingsErrors.NotBlockedByAdmin",
        description: "User is not blocked by admin. " +
        $"And you are trying to set it to '{isBlockedByAdmin.Name}.'");

    public static Error UserAlreadyDeleted(IsDeleted isDeleted) => Error.Conflict(
        code: "SettingsErrors.UserAlreadyDeleted",
        description: $"User is already deleted and you are trying to set it to '{isDeleted.Name}'");

    public static Error UserNotDeleted(IsDeleted isDeleted) => Error.Conflict(
        code: "SettingsErrors.UserNotDeleted",
        description: "User is not deleted. " +
        $"You cannot restore and you are trying to set it to '{isDeleted.Name}'");

    public static readonly Error AlreadyVerificated = Error.Conflict(
        code: "SettingsErrors.AlreadyVerificated",
        description: "The user is already verificated.");

    public static Error UnverifiedAnyway(IsVerified isVerified) => Error.Conflict(
        code: "SettingsErrors.UnverifiedAnyway",
        description: "The user is unverified anyway." +
         $"Now you try to '{isVerified.Name}'.");
}
