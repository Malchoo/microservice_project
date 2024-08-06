using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors.Profile;

public static class LastNameErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "LastNameErrors.LastNameEmpty",
        description: "Last name cannot be empty.");

    public static readonly Error LastNameTooShort = Error.Validation(
        code: "LastNameErrors.LastNameTooShort",
        description: $"Last name cannot be shorter than {LastName.MinLength} characters.");

    public static readonly Error LastNameTooLong = Error.Validation(
        code: "LastNameErrors.LastNameTooLong",
        description: $"Last name cannot be longer than {LastName.MaxLength} characters.");
}