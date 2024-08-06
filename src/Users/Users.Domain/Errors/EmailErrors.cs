using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors;

public static class EmailErrors
{
    public static Error Empty => Error.Validation(
        code: "EmailErrors.Empty",
        description: "Email is empty.");

    public static Error TooShort => Error.Validation(
        code: "EmailErrors.TooShort",
        description: $"Email must be at least {Email.MinLength} characters long.");

    public static Error TooLong => Error.Validation(
        code: "EmailErrors.TooLong",
        description: $"Email must be less than {Email.MaxLength} characters long.");

    public static Error InvalidFormat => Error.Validation(
        code: "EmailErrors.InvalidFormat",
        description: "Email format is invalid.");

    public static Error AlreadyExists => Error.Conflict(
        code: "EmailErrors.AlreadyExists",
        description: "This email already exists.");

    public static Error IsBlocked => Error.Conflict(
        code: "EmailErrors.IsBlocked",
        description: "This email is blocked.");

    public static Error InvalidCharacters => Error.Validation(
       code: "EmailErrors.InvalidCharacters",
       description: "Email contains invalid characters. Only alphanumeric characters, '.', '_', '@', and '-' are allowed.");
}
