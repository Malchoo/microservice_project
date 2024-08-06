using ErrorOr;
using Friendships.Write.Domain.ValueObjects;

namespace Friendships.Domain.DomainErrors;

public static partial class Errors
{
    public static partial class ValueObjects
    {
        public sealed class Settings
        {
            public static readonly Error NoRolesToAdd = Error.Validation(
                code: "ValueObjects.Settings.NoRolesToAdd",
                description: "No roles to add.");

            public static readonly Error RoleNotFound = Error.NotFound(
                code: "ValueObjects.Settings.RoleNotFound",
                description: "Role not found.");

            public static readonly Error CannotRemoveRoleUser = Error.Validation(
                code: "ValueObjects.Settings.CannotRemoveRoleUser",
                description: "Cannot remove user role.");

            public static readonly Error BlockedByAdmin = Error.Conflict(
                code: "ValueObjects.Settings.BlockedByAdmin",
                description: "User is blocked by admin.");

            public static Error AlreadyBlockedByAdmin(IsBlockedByAdmin isBlockedByAdmin) => Error.Conflict(
                code: "ValueObjects.Settings.AlreadyBlockedByAdmin",
                description: "User is already blocked by admin. " +
                $"Now you try to set it to '{isBlockedByAdmin.Name}'.");

            public static Error NotBlockedByAdmin(IsBlockedByAdmin isBlockedByAdmin) => Error.Conflict(
                code: "ValueObjects.Settings.NotBlockedByAdmin",
                description: "User is not blocked by admin. " +
                $"And you are trying to set it to '{isBlockedByAdmin.Name}.'");

            public static Error UserAlreadyDeleted(IsDeleted isDeleted) => Error.Conflict(
                code: "ValueObjects.Settings.UserAlreadyDeleted",
                description: $"User is already deleted and you are trying to set it to '{isDeleted.Name}'");

            public static Error UserNotDeleted(IsDeleted isDeleted) => Error.Conflict(
                code: "ValueObjects.Settings.UserNotDeleted",
                description: "User is not deleted. " +
                $"You cannot restore and you are trying to set it to '{isDeleted.Name}'");

            public static readonly Error AlreadyVerificated = Error.Conflict(
                code: "ValueObjects.Settings.AlreadyVerificated",
                description: "The user is already verificated.");
        }
    }
}
