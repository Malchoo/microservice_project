using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors.Profile;

public static class FirstNameErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "FirstNameErrors.FirstNameEmpty",
        description: "First name cannot be empty.");

    public static readonly Error FirstNameTooShort = Error.Validation(
        code: "FirstNameErrors.FirstNameTooShort",
        description: $"First name cannot be shorter than {FirstName.MinLength} characters.");

    public static readonly Error FirstNameTooLong = Error.Validation(
        code: "FirstNameErrors.FirstNameTooLong",
        description: $"First name cannot be longer than {FirstName.MaxLength} characters.");
}