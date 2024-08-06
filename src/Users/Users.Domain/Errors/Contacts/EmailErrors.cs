using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors.Contacts;
public static class EmailErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "EmailErrors.Empty",
        description: "Email is empty.");

    public static readonly Error TooShort = Error.Validation(
        code: "EmailErrors.TooShort",
        description: $"Email must be at least {Email.MinLength} characters long.");

    public static readonly Error TooLong = Error.Validation(
        code: "EmailErrors.TooLong",
        description: $"Email must be less than {Email.MaxLength} characters long.");

    public static readonly Error InvalidFormat = Error.Validation(
        code: "EmailErrors.InvalidFormat",
        description: "Email format is invalid.");

    public static readonly Error AlreadyExists = Error.Conflict(
        code: "EmailErrors.AlreadyExists",
        description: "This email already exists.");

    public static readonly Error IsBlocked = Error.Conflict(
        code: "EmailErrors.IsBlocked",
        description: "This email is blocked.");
}