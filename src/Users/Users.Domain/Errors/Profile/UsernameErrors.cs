using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors.Profile;

public static class UsernameErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "UsernameErrors.Empty",
        description: "Username is empty.");

    public static readonly Error TooShort = Error.Validation(
        code: "UsernameErrors.TooShort",
        description: $"Username must be at least {Username.MinLength} characters long.");

    public static readonly Error TooLong = Error.Validation(
        code: "UsernameErrors.TooLong",
        description: $"Username must be less than {Username.MaxLength} characters long.");

    public static readonly Error Invalid = Error.Validation(
        code: "UsernameErrors.Invalid",
        description: "Username must only contain letters.");

    public static readonly Error NoWhiteSpaces = Error.Validation(
        code: "UsernameErrors.NoWhiteSpace",
        description: "Username must not contain spaces.");

    public static readonly Error OnlyOneAlphabet = Error.Validation(
        code: "UsernameErrors.OnlyOneAlphabet",
        description: "Username must not mix different alphabets.");

    public static readonly Error AlreadyExists = Error.Conflict(
        code: "UsernameErrors.AlreadyExists",
        description: "This username already exists.");

    public static readonly Error IsBlockedByAdmin = Error.Conflict(
        code: "UsernameErrors.IsBlocked",
        description: "This username is blocked.");
}