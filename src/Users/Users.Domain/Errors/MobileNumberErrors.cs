using ErrorOr;

namespace Users.Domain.Errors;

public static class MobileNumberErrors
{
    public static Error Empty => Error.Validation(
        code: "MobileNumberErrors.Empty",
        description: "Mobile number is empty.");

    public static Error TooShort(int minLength) => Error.Validation(
        code: "MobileNumberErrors.TooShort",
        description: $"Mobile number must be at least {minLength} numbers long.");

    public static Error TooLong(int maxLength) => Error.Validation(
        code: "MobileNumberErrors.TooLong",
        description: $"Mobile number must be less than {maxLength} numbers long.");

    public static Error Invalid => Error.Validation(
        code: "MobileNumberErrors.Invalid",
        description: "Invalid mobile number format.");

    public static Error NoWhiteSpaces => Error.Validation(
        code: "MobileNumberErrors.NoWhiteSpaces",
        description: "Mobile number must not contain spaces.");

    public static Error AlreadyExists => Error.Conflict(
        code: "MobileNumberErrors.AlreadyExists",
        description: "This mobile number already exists.");

    public static Error IsBlockedByAdmin => Error.Conflict(
        code: "MobileNumberErrors.IsBlocked",
        description: "This mobile number is blocked.");
}
