using ErrorOr;

namespace Users.Domain.Errors.Preferences;

public static class TransactionAuthMethodErrors
{
    public static Error CannotConvertFromValue(int value)
        => Error.Validation(
            code: "TransactionAuthMethodErrors.CannotConvertFromValue",
            description: $"Cannot convert {value} to a valid transaction authentication method.");

    public static Error CannotChangeStatus(string currentStatus, string attemptedStatus)
        => Error.Conflict(
            code: "TransactionAuthMethodErrors.CannotChangeStatus",
            description: $"Cannot change status from {currentStatus} to {attemptedStatus}");
}
