using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors.Contacts;
public static class MobileNumberErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "MobileNumberErrors.Empty",
        description: "Mobile number is empty.");

    public static readonly Error TooShort = Error.Validation(
        code: "MobileNumberErrors.TooShort",
        description: $"Mobile number must be at least {MobileNumber.MinLength} numbers long.");

    public static readonly Error TooLong = Error.Validation(
        code: "MobileNumberErrors.TooLong",
        description: $"Mobile number must be less than {MobileNumber.MaxLength} numbers long.");

    public static readonly Error Invalid = Error.Validation(
        code: "MobileNumberErrors.Invalid",
        description: "Invalid mobile number format.");

    public static readonly Error NoWhiteSpaces = Error.Validation(
        code: "MobileNumberErrors.NoWhiteSpaces",
        description: "Mobile number must not contain spaces.");

    public static readonly Error AlreadyExists = Error.Conflict(
        code: "MobileNumberErrors.AlreadyExists",
        description: "This mobile number already exists.");

    public static readonly Error IsBlockedByAdmin = Error.Conflict(
        code: "MobileNumberErrors.IsBlocked",
        description: "This mobile number is blocked.");
}